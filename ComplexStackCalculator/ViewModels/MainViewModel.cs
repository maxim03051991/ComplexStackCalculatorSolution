using ComplexNumbersLib;                          // Библиотека комплексных чисел
using ComplexStackCalculator.Infrastructure;      // Базовые MVVM-классы и команды
using ComplexStackCalculator.Operations;          // Реестр операций калькулятора
using ComplexStackCalculator.Services;            // Сервисы стека и памяти
using ComplexStackCalculator.Utils;               // Форматирование и константы
using System;                                     // Базовые типы .NET
using System.Collections.ObjectModel;             // ObservableCollection для UI
using System.Linq;                                // LINQ, нужен для Cast
using System.Windows.Input;                       // ICommand для команд

namespace ComplexStackCalculator.ViewModels;      // Пространство имён ViewModel

public sealed class MainViewModel : ViewModelBase // Главная ViewModel калькулятора
{
    private readonly IStackService<Cx> _stack;    // Стек комплексных чисел
    private readonly IMemoryStore<Cx> _memory;    // Память калькулятора
    private readonly OperationRegistry _ops;      // Набор доступных операций

    private string _inputText = "3+4i";           // Текст в поле ввода
    private string _status = "";                  // Строка состояния
    private ComplexDisplayMode _displayMode = ComplexDisplayMode.Algebraic; // Режим показа числа

    public MainViewModel()                        // Конструктор ViewModel
    {
        _stack = new StackService<Cx>();          // Создаём стек
        _memory = new MemoryStore<Cx>();          // Создаём память
        _ops = new OperationRegistry();           // Создаём реестр операций

        StackItems = new ObservableCollection<ComplexItem>(); // Коллекция элементов стека для UI
        DisplayModes = new ObservableCollection<ComplexDisplayMode>( // Коллекция режимов отображения
            Enum.GetValues(typeof(ComplexDisplayMode)).Cast<ComplexDisplayMode>() // Все значения enum
        );

        EnterCommand = new RelayCommand(Enter);   // Команда ввода числа
        ClearInputCommand = new RelayCommand(() => InputText = ""); // Очистить ввод
        BackspaceCommand = new RelayCommand(Backspace); // Удалить последний символ

        ClearStackCommand = new RelayCommand(ClearStack); // Очистить стек
        ClearStatusCommand = new RelayCommand(() => Status = ""); // Очистить статус

        OpCommand = new RelayCommand(p => ExecuteOp(p as string)); // Выполнить операцию
        PushConstCommand = new RelayCommand(p => PushConst(p as string)); // Поместить константу

        MemorySaveCommand = new RelayCommand(MemorySave); // Сохранить верх стека в память
        MemoryRecallCommand = new RelayCommand(MemoryRecall); // Загрузить из памяти
        MemoryClearCommand = new RelayCommand(() =>        // Очистить память
        {
            _memory.Clear();                               // Очистка памяти
            Status = "Memory cleared.";                    // Сообщение пользователю
        });

        PutTopToInputCommand = new RelayCommand(PutTopToInput); // Верх стека в поле ввода
        HelpCommand = new RelayCommand(ShowHelp);         // Показать справку

        RefreshStackView();                               // Обновить отображение стека
        Status = "Форматы: 3+4i, 3-4i, 4i, -i, 3, (3,4), (3;4). Дробная часть: точка '.'"; // Стартовая подсказка
    }

    public ObservableCollection<ComplexItem> StackItems { get; } // Данные стека для интерфейса
    public ObservableCollection<ComplexDisplayMode> DisplayModes { get; } // Доступные режимы отображения

    public string InputText                              // Свойство текста ввода
    {
        get => _inputText;                               // Вернуть текущее значение
        set => Set(ref _inputText, value);               // Изменить и уведомить UI
    }

    public string Status                                 // Свойство строки состояния
    {
        get => _status;                                  // Вернуть текущий статус
        set => Set(ref _status, value);                  // Изменить и уведомить UI
    }

    public ComplexDisplayMode DisplayMode                // Свойство режима отображения
    {
        get => _displayMode;                             // Вернуть текущий режим
        set
        {
            if (Set(ref _displayMode, value))            // Если режим изменился
                RefreshStackView();                      // Обновить отображение стека
        }
    }

    public ICommand EnterCommand { get; }                // Команда ввода числа
    public ICommand ClearInputCommand { get; }           // Команда очистки ввода
    public ICommand BackspaceCommand { get; }            // Команда backspace

    public ICommand ClearStackCommand { get; }           // Команда очистки стека
    public ICommand ClearStatusCommand { get; }          // Команда очистки статуса

    public ICommand OpCommand { get; }                   // Команда выполнения операции
    public ICommand PushConstCommand { get; }            // Команда добавления константы

    public ICommand MemorySaveCommand { get; }           // Команда сохранить в память
    public ICommand MemoryRecallCommand { get; }         // Команда загрузить из памяти
    public ICommand MemoryClearCommand { get; }          // Команда очистить память

    public ICommand PutTopToInputCommand { get; }        // Команда верх стека -> ввод
    public ICommand HelpCommand { get; }                 // Команда справки

    private void Enter()                                 // Ввести число из строки в стек
    {
        try                                              // Начало обработки ошибок
        {
            Status = "";                                 // Очистить статус
            var z = Cx.Parse(InputText);                 // Распарсить строку в комплексное число
            _stack.Push(z);                              // Положить число в стек
            RefreshStackView();                          // Обновить отображение стека
        }
        catch (Exception ex)                             // Если возникла ошибка
        {
            Status = $"Enter error: {ex.Message}";       // Показать сообщение об ошибке
        }
    }

    private void Backspace()                             // Удалить последний символ из ввода
    {
        if (string.IsNullOrEmpty(InputText)) return;     // Если строка пустая, выйти
        InputText = InputText[..^1];                     // Взять строку без последнего символа
    }

    private void ClearStack()                            // Очистить стек
    {
        _stack.Clear();                                  // Очистка стека
        RefreshStackView();                              // Обновить UI
        Status = "Stack cleared.";                       // Сообщение пользователю
    }

    private void PushConst(string? key)                  // Положить константу в стек
    {
        try                                              // Начало обработки ошибок
        {
            Status = "";                                 // Очистить статус

            Cx z = key switch                            // Выбрать константу по ключу
            {
                "pi" => new Cx(MathConst.Pi, 0),         // Число π
                "e" => new Cx(MathConst.E, 0),           // Число e
                "i" => Cx.I,                             // Мнимая единица
                "0" => Cx.Zero,                          // Ноль
                "1" => Cx.One,                           // Единица
                _ => throw new ArgumentException("Unknown constant.") // Неизвестная константа
            };

            _stack.Push(z);                              // Положить константу в стек
            RefreshStackView();                          // Обновить отображение стека
        }
        catch (Exception ex)                             // Если возникла ошибка
        {
            Status = $"Const error: {ex.Message}";       // Показать сообщение об ошибке
        }
    }

    private void MemorySave()                            // Сохранить верх стека в память
    {
        try                                              // Начало обработки ошибок
        {
            Status = "";                                 // Очистить статус
            var z = _stack.Peek();                       // Взять верхний элемент стека
            _memory.Save(z);                             // Сохранить его в память
            Status = $"Saved to memory: {ComplexFormat.Format(z, DisplayMode)}"; // Показать сохранённое значение
        }
        catch (Exception ex)                             // Если возникла ошибка
        {
            Status = $"MS error: {ex.Message}";          // Показать сообщение об ошибке
        }
    }

    private void MemoryRecall()                          // Загрузить значение из памяти в стек
    {
        try                                              // Начало обработки ошибок
        {
            Status = "";                                 // Очистить статус

            if (_memory.TryLoad(out var z))              // Если значение в памяти есть
            {
                _stack.Push(z);                          // Положить его в стек
                RefreshStackView();                      // Обновить отображение стека
                Status = $"Recalled: {ComplexFormat.Format(z, DisplayMode)}"; // Показать загруженное значение
            }
            else                                         // Если память пуста
            {
                Status = "Memory empty.";                // Сообщить пользователю
            }
        }
        catch (Exception ex)                             // Если возникла ошибка
        {
            Status = $"MR error: {ex.Message}";          // Показать сообщение об ошибке
        }
    }

    private void PutTopToInput()                         // Поместить верх стека в поле ввода
    {
        try                                              // Начало обработки ошибок
        {
            Status = "";                                 // Очистить статус
            var z = _stack.Peek();                       // Взять верхний элемент стека
            InputText = z.ToString();                    // Перевести его в строку и записать во ввод
            Status = "Top -> Input (algebraic).";        // Сообщить пользователю
        }
        catch (Exception ex)                             // Если возникла ошибка
        {
            Status = $"PutTop error: {ex.Message}";      // Показать сообщение об ошибке
        }
    }

    private void ShowHelp()                              // Показать краткую справку
    {
        Status =                                         // Записать текст справки в статус
            "Ввод: 3+4i, 3-4i, 4i, -i, 3, 0.5+2.1i, (3,4), (3;4). " +
            "Операции работают со стеком: бинарные снимают 2 значения, унарные — 1.";
    }

    private void ExecuteOp(string? key)                  // Выполнить операцию по ключу
    {
        if (string.IsNullOrWhiteSpace(key)) return;      // Если ключ пустой, выйти

        try                                              // Начало обработки ошибок
        {
            Status = "";                                 // Очистить статус

            switch (key)                                 // Проверка специальных стековых команд
            {
                case "swap":                             // Обмен двух верхних элементов
                    SwapTop2();                          // Выполнить swap
                    RefreshStackView();                  // Обновить UI
                    return;                              // Завершить метод

                case "dup":                              // Дублирование верхнего элемента
                    Dup();                               // Выполнить dup
                    RefreshStackView();                  // Обновить UI
                    return;                              // Завершить метод

                case "drop":                             // Удаление верхнего элемента
                    Drop();                              // Выполнить drop
                    RefreshStackView();                  // Обновить UI
                    return;                              // Завершить метод
            }

            if (!_ops.TryGet(key, out var op))           // Попробовать найти операцию в реестре
                throw new InvalidOperationException($"Unknown operation key: {key}"); // Ошибка, если нет операции

            if (op.Arity == 1)                           // Если операция унарная
            {
                var a = _stack.Pop();                    // Снять один аргумент со стека
                var r = op.Execute(a);                   // Выполнить операцию
                _stack.Push(r);                          // Положить результат обратно в стек
            }
            else                                         // Если операция бинарная
            {
                var b = _stack.Pop();                    // Снять верхний элемент
                var a = _stack.Pop();                    // Снять следующий элемент
                var r = op.Execute(a, b);                // Выполнить операцию над двумя аргументами
                _stack.Push(r);                          // Положить результат в стек
            }

            RefreshStackView();                          // Обновить отображение стека
        }
        catch (Exception ex)                             // Если возникла ошибка
        {
            Status = $"Op '{key}' error: {ex.Message}";  // Показать сообщение об ошибке
        }
    }

    private void SwapTop2()                              // Поменять местами два верхних элемента
    {
        if (_stack.Count < 2)                            // Если элементов меньше двух
            throw new InvalidOperationException("Need 2 values to swap."); // Бросить ошибку

        var a = _stack.Pop();                            // Снять верхний элемент
        var b = _stack.Pop();                            // Снять второй элемент
        _stack.Push(a);                                  // Вернуть первый снятый
        _stack.Push(b);                                  // Вернуть второй снятый
    }

    private void Dup()                                   // Дублировать верхний элемент
    {
        var a = _stack.Peek();                           // Взять верхний элемент без удаления
        _stack.Push(a);                                  // Добавить его копию наверх
    }

    private void Drop()                                  // Удалить верхний элемент
    {
        _stack.Pop();                                    // Снять верхний элемент со стека
    }

    private void RefreshStackView()                      // Обновить отображаемый список стека
    {
        StackItems.Clear();                              // Очистить текущий список для UI

        var items = _stack.ItemsTopFirst;                // Получить элементы стека сверху вниз
        for (int i = 0; i < items.Count; i++)            // Пройти по всем элементам
        {
            var z = items[i];                            // Взять текущее комплексное число
            var display = ComplexFormat.Format(z, DisplayMode); // Преобразовать в строку для показа
            StackItems.Add(new ComplexItem(i, z, display)); // Добавить элемент в коллекцию UI
        }
    }
}