
using ComplexNumbersLib;
using System.Numerics;

namespace ComplexStackCalculator.Utils;

// static означает, что класс содержит только статические методы
// и его нельзя создать через new
public static class ComplexFormat
{
    // Статический метод, который форматирует комплексное число в строку
    // z — комплексное число
    // mode — режим отображения числа
    // digits — количество знаков после запятой (по умолчанию 4)
    public static string Format(Cx z, ComplexDisplayMode mode, int digits = 4)
    {
        // switch expression — современная форма switch в C#
        // в зависимости от режима отображения возвращается разная строка
        return mode switch
        {
            // Алгебраическая форма: a + bi
            // Используется стандартный метод ToString()
            ComplexDisplayMode.Algebraic => z.ToString(),

            // Полярная форма: r∠θ
            // Вызываем метод преобразования в полярную форму
            // digits определяет точность округления
            ComplexDisplayMode.Polar => z.ToPolarString(digits),

            // Экспоненциальная форма: r * e^(iθ)
            // Также передаём количество знаков после запятой
            ComplexDisplayMode.Exponential => z.ToExponentialString(digits),

            // Если режим неизвестен — используем обычную строковую форму
            _ => z.ToString()
        };
    }
}