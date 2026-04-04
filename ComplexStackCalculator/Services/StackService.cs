using System;
using System.Collections.Generic;

namespace ComplexStackCalculator.Services;

// sealed означает, что от этого класса нельзя наследоваться
public sealed class StackService<T> : IStackService<T>
{
    // Внутренний список для хранения элементов стека
    // Верхушка стека находится в конце списка
    private readonly List<T> _list = new();

    // Свойство, возвращающее количество элементов в стеке
    public int Count => _list.Count;

    // Свойство, которое возвращает элементы стека,
    // начиная с верхнего (Top → Bottom)
    public IReadOnlyList<T> ItemsTopFirst
    {
        get
        {
            // Создаём новый массив такого же размера как стек
            var snap = new T[_list.Count];

            // Проходим по всем элементам списка
            for (int i = 0; i < _list.Count; i++)
                // Копируем элементы в обратном порядке
                // чтобы верхушка стека была первой
                snap[i] = _list[_list.Count - 1 - i];

            // Возвращаем массив (только для чтения)
            return snap;
        }
    }

    // Метод добавляет элемент в стек (наверх)
    public void Push(T value) => _list.Add(value);

    // Метод удаляет верхний элемент стека и возвращает его
    public T Pop()
    {
        // Если стек пустой — вызываем исключение
        if (_list.Count == 0)
            throw new InvalidOperationException("Stack is empty.");

        // Получаем индекс последнего элемента
        int last = _list.Count - 1;

        // Сохраняем значение верхнего элемента
        T v = _list[last];

        // Удаляем верхний элемент из списка
        _list.RemoveAt(last);

        // Возвращаем удалённый элемент
        return v;
    }

    // Метод возвращает верхний элемент стека,
    // но не удаляет его
    public T Peek()
    {
        // Проверяем пуст ли стек
        if (_list.Count == 0)
            throw new InvalidOperationException("Stack is empty.");

        // ^1 означает "первый элемент с конца"
        // то есть верхушка стека
        return _list[^1];
    }

    // Метод полностью очищает стек
    public void Clear() => _list.Clear();
}