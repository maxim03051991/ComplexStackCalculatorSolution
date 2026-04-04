using ComplexNumbersLib;      // Библиотека комплексных чисел (тип Cx)
using System;                 // Базовые классы .NET
using System.Numerics;        // Числовые типы (возможно используется внутри)

namespace ComplexStackCalculator.Operations;

// Класс бинарной операции (операция с двумя аргументами)
// sealed — запрещает наследование
public sealed class BinaryOperation : ICalcOperation
{
    // Делегат функции операции:
    // принимает два комплексных числа и возвращает одно
    private readonly Func<Cx, Cx, Cx> _fn;

    // Ключ операции (например "+", "-", "*", "/")
    public string Key { get; }

    // Арность — количество аргументов операции (2 для бинарной)
    public int Arity => 2;

    // Конструктор
    // key — строковое обозначение операции
    // fn — функция, которая выполняет операцию
    public BinaryOperation(string key, Func<Cx, Cx, Cx> fn)
    {
        Key = key;   // сохраняем ключ
        _fn = fn;    // сохраняем функцию операции
    }

    // Метод выполнения операции
    // a — первый аргумент
    // b — второй аргумент (может быть null)
    public Cx Execute(Cx a, Cx? b = null)
    {
        // Если второй аргумент не передан — ошибка
        if (b is null)
            throw new ArgumentNullException(nameof(b));

        // Выполняем функцию операции и возвращаем результат
        return _fn(a, b.Value);
    }
}