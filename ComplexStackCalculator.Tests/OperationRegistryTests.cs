using ComplexStackCalculator.Operations;

namespace ComplexStackCalculator.Tests;

// Класс с тестами для OperationRegistry
public class OperationRegistryTests
{
    [Fact]

    // Тест проверяет, что в реестре есть базовые операции
    public void Registry_HasBasicOps()
    {
        // Создаём экземпляр реестра операций
        var reg = new OperationRegistry();

        // Проверяем, что операция "+" существует
        Assert.True(reg.TryGet("+", out var add));

        // Проверяем, что операция "+" принимает 2 аргумента
        Assert.Equal(2, add.Arity);

        // Проверяем, что операция "sin" существует
        Assert.True(reg.TryGet("sin", out var sin));

        // Проверяем, что операция "sin" принимает 1 аргумент
        Assert.Equal(1, sin.Arity);

        // Проверяем, что операция "inv" существует
        Assert.True(reg.TryGet("inv", out var inv));

        // Проверяем, что операция "inv" принимает 1 аргумент
        Assert.Equal(1, inv.Arity);
    }

    [Fact]
    // Тест проверяет корректность работы операции сложения
    public void Add_ExecutesCorrectly()
    {
        // Создаём реестр операций
        var reg = new OperationRegistry();

        // Получаем операцию "+"
        Assert.True(reg.TryGet("+", out var add));

        // Выполняем сложение комплексных чисел:
        // (1 + 2i) + (3 - 4i)
        var r = add.Execute(new Cx(1, 2), new Cx(3, -4));

        // Проверяем, что результат равен (4 - 2i)
        // AlmostEquals используется для сравнения с учётом погрешности
        Assert.True(r.AlmostEquals(new Cx(4, -2), 1e-12));
    }

    [Fact]
    // Тест проверяет корректность работы операции обратного числа
    public void Inv_ExecutesCorrectly()
    {
        // Создаём реестр операций
        var reg = new OperationRegistry();

        // Получаем операцию "inv"
        Assert.True(reg.TryGet("inv", out var inv));

        // Выполняем операцию:
        // inv(2 + 0i) = 1 / 2
        var r = inv.Execute(new Cx(2, 0));

        // Проверяем, что результат равен (0.5 + 0i)
        Assert.True(r.AlmostEquals(new Cx(0.5, 0), 1e-12));
    }
}