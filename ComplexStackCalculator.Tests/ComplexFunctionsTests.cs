namespace ComplexStackCalculator.Tests;

public class ComplexFunctionsTests
// Класс, содержащий тесты математических функций комплексных чисел
{
    [Fact]
    //  отдельный тест
    public void Sin_Zero_IsZero()
    // Тест: sin(0) должен быть равен 0
    {
        var r = Cx.Sin(Cx.Zero);
        // Вычисляем синус комплексного нуля

        Assert.True(r.AlmostEquals(Cx.Zero, 1e-12));
        // Проверяем, что результат почти равен 0 с точностью 10^-12
    }

    [Fact]
    // Отдельный тест
    public void Cos_Zero_IsOne()
    // Тест: cos(0) должен быть равен 1
    {
        var r = Cx.Cos(Cx.Zero);
        // Вычисляем косинус комплексного нуля

        Assert.True(r.AlmostEquals(Cx.One, 1e-12));
        // Проверяем, что результат почти равен 1
    }

    [Fact]
    // Отдельный тест
    public void Tan_Zero_IsZero()
    // Тест: tan(0) должен быть равен 0
    {
        var r = Cx.Tan(Cx.Zero);
        // Вычисляем тангенс комплексного нуля

        Assert.True(r.AlmostEquals(Cx.Zero, 1e-12));
        // Проверяем, что результат почти равен 0
    }

    [Fact]
    // Отдельный тест
    public void Sqrt_Of4_Is2()
    // Тест: квадратный корень из 4 должен быть равен 2
    {
        var r = Cx.Sqrt(new Cx(4, 0));
        // Вычисляем sqrt(4 + 0i)

        Assert.True(r.AlmostEquals(new Cx(2, 0), 1e-12));
        // Проверяем, что результат равен 2 + 0i
    }

    [Fact]
    // Отдельный тест
    public void Ln_Of1_Is0()
    // Тест: ln(1) должен быть равен 0
    {
        var r = Cx.Ln(Cx.One);
        // Вычисляем натуральный логарифм числа 1

        Assert.True(r.AlmostEquals(Cx.Zero, 1e-12));
        // Проверяем, что результат почти равен 0
    }

    [Fact]
    // Отдельный тест
    public void Exp_Ln_Z_IsZ_ForNonZero()
    // Тест: exp(ln(z)) = z для ненулевого комплексного числа
    {
        var z = new Cx(1.2, -0.7);
        // Создаём комплексное число z = 1.2 - 0.7i

        var r = Cx.Exp(Cx.Ln(z));
        // Сначала вычисляем ln(z), затем exp(ln(z))

        Assert.True(r.AlmostEquals(z, 1e-9));
        // Проверяем, что результат почти равен исходному числу z
        // Используется точность 10^-9 из-за возможных ошибок вычислений
    }
}