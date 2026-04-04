namespace ComplexStackCalculator.Services;

// sealed — запрещает наследование от этого класса.
// MemoryStore<T> — обобщённый класс, который может хранить значение любого типа T.
// : IMemoryStore<T> — класс реализует интерфейс IMemoryStore.
public sealed class MemoryStore<T> : IMemoryStore<T>
{
    // Флаг, показывающий есть ли сохранённое значение
    private bool _has;

    // Поле для хранения значения.
    // T? означает, что значение может быть null.
    private T? _value;

    // Свойство только для чтения.
    // Возвращает true если значение есть, иначе false.
    public bool HasValue => _has;

    // Метод сохранения значения в память.
    public void Save(T value)
    {
        // Сохраняем значение
        _value = value;

        // Отмечаем, что значение теперь существует
        _has = true;
    }

    // Метод попытки загрузить значение из памяти.
    // out T value — параметр через который возвращается значение.
    // Метод возвращает true если значение удалось получить.
    public bool TryLoad(out T value)
    {
        // Проверяем:
        // 1. Было ли сохранено значение (_has)
        // 2. Значение не равно null
        if (_has && _value is not null)
        {
            // Передаём сохранённое значение наружу
            value = _value;

            // Сообщаем что операция успешна
            return true;
        }

        // Если значения нет, возвращаем значение по умолчанию
        // default для разных типов:
        // int → 0
        // double → 0.0
        // string → null
        value = default!;

        // Сообщаем что значение получить не удалось
        return false;
    }

    // Метод очистки памяти
    public void Clear()
    {
        // Помечаем что значение отсутствует
        _has = false;

        // Сбрасываем значение к значению по умолчанию
        _value = default;
    }
}