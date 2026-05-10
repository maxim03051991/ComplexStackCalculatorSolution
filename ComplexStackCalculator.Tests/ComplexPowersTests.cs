namespace ComplexStackCalculator.Tests;
// Публичный класс, содержащий тесты для операций возведения в степень
public class ComplexPowersTests
{
    [Fact]
    // Тест метода Square()
    public void Square_Works()
    {
        // Создаём комплексное число z = 2 + 3i
        var z = new Cx(2, 3);

        // Вызываем метод Square(), который должен возвести число в квадрат
        // (2 + 3i)^2 = 4 + 12i + 9i^2 = 4 + 12i - 9 = -5 + 12i
        var r = z.Square(); // -5 + 12i

        // Проверяем, что результат почти равен ожидаемому значению
        // AlmostEquals используется из-за возможных ошибок вычислений с double
        Assert.True(r.AlmostEquals(new Cx(-5, 12), 1e-12));
    }

    [Fact]
    // Тест метода Cube()
    public void Cube_Works()
    {
        // Создаём комплексное число 1 + i
        var z = new Cx(1, 1);

        // Возводим число в куб:
        // (1 + i)^3 = -2 + 2i
        var r = z.Cube(); // -2 + 2i

        // Проверяем корректность результата
        Assert.True(r.AlmostEquals(new Cx(-2, 2), 1e-12));
    }

    [Fact]
    // Тест возведения в целую степень
    public void Pow_Int_Works()
    {
        // Комплексное число 2 + 0i
        var z = new Cx(2, 0);

        // Возводим число в степень 10
        // 2^10 = 1024
        var r = z.Pow(10);

        // Проверяем результат
        Assert.True(r.AlmostEquals(new Cx(1024, 0), 1e-12));
    }

    [Fact]
    // Тест возведения в вещественную степень
    public void Pow_Double_Works()
    {
        // Комплексное число 9 + 0i
        var z = new Cx(9, 0);

        // Возводим число в степень 0.5 (извлекаем квадратный корень)
        // sqrt(9) = 3
        var r = z.Pow(0.5);

        // Проверяем результат с менее строгой точностью
        Assert.True(r.AlmostEquals(new Cx(3, 0), 1e-9));
    }

    [Fact]
    // Проверочный (smoke) тест для комплексной степени
    public void Pow_Complex_SmokeTest()
    {
        // Основание степени: z = 1 + i
        var z = new Cx(1, 1);

        // Показатель степени: w = 2 - i
        var w = new Cx(2, -1);

        // Вычисляем z^w
        var r = z.Pow(w);

        // Ожидаемый результат вычисляется по формуле:
        // z^w = exp(w * ln(z))
        // Это стандартное определение комплексной степени
        var expected = Cx.Exp(w * Cx.Ln(z));

        // Сравниваем вычисленный результат с ожидаемым
        Assert.True(r.AlmostEquals(expected, 1e-9));
    }
}