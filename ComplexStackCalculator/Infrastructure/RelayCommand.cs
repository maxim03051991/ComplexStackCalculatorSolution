using System;
using System.Windows.Input;

namespace ComplexStackCalculator.Infrastructure
{
    /*
     Реализация интерфейса ICommand.
     Применяется в паттерне MVVM для связывания элементов интерфейса
     (кнопок, меню и т.п.) с логикой во ViewModel.
     Позволяет задать:
     — действие, которое выполнится при вызове команды
     — условие, можно ли выполнять команду
    */
    public sealed class RelayCommand : ICommand
    {
        // Делегат, содержащий код выполнения команды.
        // object? — параметр команды (может быть null).
        private readonly Action<object?> _execute;

        // Делегат, определяющий доступность команды.
        // Если не задан — команда доступна всегда.
        private readonly Func<object?, bool>? _canExecute;

        // Конструктор для команды без параметров.
        // Позволяет передать обычный Action (без аргументов).
        public RelayCommand(Action execute)
            // Оборачиваем Action в Action<object?>,
            // игнорируя входной параметр
            : this(_ => execute(), null)
        {
        }

        /*
         Основной конструктор.

         execute — метод, выполняемый при вызове команды
         canExecute — метод, определяющий, можно ли выполнить команду
                      (необязательный параметр)
        */
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            // Проверка, чтобы метод выполнения не был null
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Определяет, можно ли выполнить команду.
        // Вызывается системой для обновления состояния элементов UI.
        public bool CanExecute(object? parameter)
            => _canExecute?.Invoke(parameter) ?? true;

        // Выполняет команду.
        public void Execute(object? parameter)
            => _execute(parameter);

        /*
         Событие уведомляет интерфейс,
         что доступность команды изменилась.
         Например, чтобы кнопка стала активной или неактивной.
        */
        public event EventHandler? CanExecuteChanged;

        // Метод для вызова события вручную,
        // когда изменились условия выполнения команды.
        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}