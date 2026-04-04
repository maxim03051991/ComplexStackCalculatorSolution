using ComplexNumbersLib;
using System.Numerics;


namespace ComplexStackCalculator.Utils;

// sealed означает, что этот класс нельзя наследовать
public sealed class ComplexItem
{
    // Свойство только для чтения.
    // Хранит индекс элемента относительно вершины стека
    // (например: 0 — верхний элемент, 1 — следующий и т.д.)
    public int IndexFromTop { get; }

    // Свойство для хранения значения комплексного числа
    public Cx Value { get; }

    // Строковое представление комплексного числа
    // (например: "3 + 4i", "5∠53°" и т.д.)
    public string Display { get; }

    // Вычисляемое свойство.
    // Возвращает строку с индексом в квадратных скобках
    // Например: [0], [1], [2]
    public string IndexLabel => $"[{IndexFromTop}]";

    // Конструктор класса.
    // Вызывается при создании объекта ComplexItem
    public ComplexItem(int indexFromTop, Cx value, string display)
    {
        // Сохраняем индекс элемента в стеке
        IndexFromTop = indexFromTop;

        // Сохраняем само комплексное число
        Value = value;

        // Сохраняем строковое отображение числа
        Display = display;
    }
}