using ComplexNumbersLib;                 // Тип Cx и операции над комплексными числами
using System.Collections.Generic;        // Dictionary
using System.Numerics;                  // (в этом файле напрямую не используется)

namespace ComplexStackCalculator.Operations;

// Реестр всех доступных операций калькулятора.
// Хранит сопоставление: "ключ операции" -> объект операции (ICalcOperation).
public sealed class OperationRegistry
{
    // Словарь операций.
    // Пример: "+" -> BinaryOperation, "sin" -> UnaryOperation.
    private readonly Dictionary<string, ICalcOperation> _ops = new();

    // Конструктор: при создании реестра сразу регистрируем набор операций.
    public OperationRegistry()
    {
        // --- Бинарные операции (2 аргумента): a op b ---
        // В стековом калькуляторе обычно b = верх стека (top),
        // a = элемент под ним (below-top).
        Register(new BinaryOperation("+", (a, b) => a + b));
        Register(new BinaryOperation("-", (a, b) => a - b));
        Register(new BinaryOperation("*", (a, b) => a * b));
        Register(new BinaryOperation("/", (a, b) => a / b));

        // --- Унарные операции (1 аргумент): op(z) ---
        Register(new UnaryOperation("inv", z => z.Reciprocal())); // 1/z

        Register(new UnaryOperation("sin", z => Cx.Sin(z)));
        Register(new UnaryOperation("cos", z => Cx.Cos(z)));
        Register(new UnaryOperation("tan", z => Cx.Tan(z)));
        Register(new UnaryOperation("sqrt", z => Cx.Sqrt(z)));
        Register(new UnaryOperation("exp", z => Cx.Exp(z)));
        Register(new UnaryOperation("ln", z => Cx.Ln(z)));

        // Быстрые степени
        Register(new UnaryOperation("z2", z => z.Square())); // z^2
        Register(new UnaryOperation("z3", z => z.Cube()));   // z^3

        // Модуль и аргумент возвращаем как действительное число в форме Cx(x, 0)
        Register(new UnaryOperation("abs", z => new Cx(z.Magnitude, 0))); // |z|
        Register(new UnaryOperation("arg", z => new Cx(z.Argument, 0)));  // arg(z)

        // Сопряжение и унарный минус
        Register(new UnaryOperation("conj", z => z.Conjugate()));
        Register(new UnaryOperation("neg", z => -z));

        // --- Возведение в степень (2 аргумента) ---
        // Ключи разные, но сигнатура одинаковая: (Cx, Cx) -> Cx
        Register(new BinaryOperation("zPowY", (z, y) => z.Pow(y)));
        Register(new BinaryOperation("zPowZ", (z, z1) => z.Pow(z1)));
    }

    // Зарегистрировать операцию по её ключу.
    // Если ключ уже существует — перезаписать операцию.
    public void Register(ICalcOperation op)
    {
        _ops[op.Key] = op;
    }

    // Попытаться получить операцию по ключу.
    // Возвращает true/false, а найденную операцию кладёт в out-параметр op.
    public bool TryGet(string key, out ICalcOperation op)
    {
        return _ops.TryGetValue(key, out op!);
    }
}