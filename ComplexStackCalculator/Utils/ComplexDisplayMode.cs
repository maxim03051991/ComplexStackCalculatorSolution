namespace ComplexStackCalculator.Utils;

// Определяет режим отображения комплексного числа.
public enum ComplexDisplayMode
{
    // Алгебраическая форма: a + bi>
    Algebraic,
    // Полярная форма: r (cos φ + i sin φ)
    Polar,
    // Показательная форма: r * e^(iφ)
    Exponential
}