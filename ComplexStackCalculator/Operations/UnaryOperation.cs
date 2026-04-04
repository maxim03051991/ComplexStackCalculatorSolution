using ComplexNumbersLib;   // Тип Cx (комплексные числа)
using System;              // Func<>, исключения и базовые типы
using System.Numerics;     // Числовые типы .NET 

namespace ComplexStackCalculator.Operations;

// Унарная операция: работает с одним аргументом (например sin(a), -a, conj(a))
public sealed class UnaryOperation : ICalcOperation
{
    // Делегат операции: принимает одно Cx и возвращает одно Cx
    private readonly Func<Cx, Cx> _fn;

    // Ключ операции (например "sin", "neg", "conj")
    public string Key { get; }

    // Арность унарной операции всегда 1
    public int Arity => 1;

    // Конструктор:
    // key — ключ операции
    // fn — функция, реализующая операцию
    public UnaryOperation(string key, Func<Cx, Cx> fn)
    {
        Key = key;   // сохраняем ключ
        _fn = fn;    // сохраняем функцию операции
    }

    // Выполнение операции:
    // b есть в сигнатуре из-за интерфейса, но для унарной операции не нужен
    public Cx Execute(Cx a, Cx? b = null)
    {
        return _fn(a); // применяем функцию к первому аргументу
    }
}