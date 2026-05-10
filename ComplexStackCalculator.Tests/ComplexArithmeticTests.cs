namespace ComplexStackCalculator.Tests;

public class ComplexArithmeticTests // Класс, содержащий тесты арифметики комплексных чисел
{
    [Fact] //  отдельный тест
    public void Add_Works() // Тест проверки сложения комплексных чисел
    {
        var a = new Cx(1, 2); // Создаём комплексное число: 1 + 2i
        var b = new Cx(3, -4); // Создаём комплексное число: 3 - 4i
        var r = a + b; // Складываем числа: (1+2i) + (3-4i)

        Assert.True(r.AlmostEquals(new Cx(4, -2)));
        // Проверяем, что результат равен 4 - 2i
    }

    [Fact] // Отдельный тест
    public void Sub_Works() // Тест проверки вычитания
    {
        var a = new Cx(1, 2); // Комплексное число 1 + 2i
        var b = new Cx(3, -4); // Комплексное число 3 - 4i
        var r = a - b; // Вычитание: (1+2i) - (3-4i)

        Assert.True(r.AlmostEquals(new Cx(-2, 6)));
        // Проверяем, что результат равен -2 + 6i
    }

    [Fact] // Отдельный тест
    public void Mul_Works() // Тест проверки умножения
    {
        var a = new Cx(1, 2); // Комплексное число 1 + 2i
        var b = new Cx(3, -4); // Комплексное число 3 - 4i

        var r = a * b; // Умножение комплексных чисел
        // (1+2i)(3-4i) = 3 -4i +6i -8i² = 11 +2i

        Assert.True(r.AlmostEquals(new Cx(11, 2)));
        // Проверяем, что результат равен 11 + 2i
    }

    [Fact] // Отдельный тест
    public void Div_Works() // Тест проверки деления
    {
        var a = new Cx(1, 2); // Комплексное число 1 + 2i
        var b = new Cx(3, -4); // Комплексное число 3 - 4i

        var r = a / b; // Деление комплексных чисел
        // Результат должен быть -0.2 + 0.4i

        Assert.True(r.AlmostEquals(new Cx(-0.2, 0.4), 1e-12));
        // Проверяем результат с точностью 10^-12
    }

    [Fact] // Отдельный тест
    public void Reciprocal_ThrowsOnZero()
    // Тест проверки выброса исключения при попытке найти обратное число для 0
    {
        Assert.Throws<DivideByZeroException>(() => Cx.Zero.Reciprocal());
        // Проверяем, что вызывается исключение DivideByZeroException
    }

    [Fact] // Отдельный тест
    public void Div_ThrowsOnZero()
    // Тест проверки деления на ноль
    {
        var a = new Cx(1, 2); // Комплексное число 1 + 2i

        Assert.Throws<DivideByZeroException>(() => _ = a / Cx.Zero);
        // Проверяем, что при делении на 0 выбрасывается DivideByZeroException
    }
}