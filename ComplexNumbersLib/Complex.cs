
using System; // Подключаем базовые типы .NET
using System.Globalization; // Подключаем CultureInfo для парсинга чисел с точкой и инвариантной культурой

namespace ComplexNumbersLib // Объявляем пространство имён библиотеки
{
    /// Структура комплексного числа z = Re + i*Im. 
    /// Реализовано: +, -, *, /, 1/z, |z|, Arg(z), Conjugate, полярная/показательная формы, 
    /// Sin/Cos/Tan, Sqrt, Exp, Ln, степени (int/double/Complex), Parse/TryParse. 
    public readonly struct Complex : IEquatable<Complex> // Объявляем неизменяемую структуру и интерфейс равенства
    {
        public double Re { get; } // Действительная часть
        public double Im { get; } // Мнимая часть

        public Complex(double re, double im) // Конструктор с двумя параметрами
        {
            Re = re; // Сохраняем действительную часть
            Im = im; // Сохраняем мнимую часть
        }

        // ===== Константы комплексных чисел ===== // Раздел констант
        public static readonly Complex Zero = new Complex(0, 0); // 0 + 0i
        public static readonly Complex One = new Complex(1, 0); // 1 + 0i
        public static readonly Complex I = new Complex(0, 1); // 0 + 1i

        // ===== Базовые свойства ===== // Раздел базовых свойств
        public double Magnitude => Math.Sqrt(Re * Re + Im * Im); // |z| = sqrt(Re^2 + Im^2)
        public double MagnitudeSquared => Re * Re + Im * Im; // |z|^2 = Re^2 + Im^2
        public double Argument => Math.Atan2(Im, Re); // Arg(z) = atan2(Im, Re) в (-pi, pi]

        public Complex Conjugate() => new Complex(Re, -Im); // Сопряжённое число: Re - iIm

        public Complex Reciprocal() // Метод для 1/z
        {
            double denom = MagnitudeSquared; // denom = Re^2 + Im^2
            if (denom == 0.0) // Проверяем деление на ноль
                throw new DivideByZeroException("Reciprocal of 0 is undefined."); // Бросаем исключение
            return new Complex(Re / denom, -Im / denom); // (Re - iIm) / (Re^2 + Im^2)
        }

        // ===== Операторы ===== // Раздел операторов
        public static Complex operator +(Complex a, Complex b) => new Complex(a.Re + b.Re, a.Im + b.Im); // Сложение
        public static Complex operator -(Complex a, Complex b) => new Complex(a.Re - b.Re, a.Im - b.Im); // Вычитание
        public static Complex operator -(Complex a) => new Complex(-a.Re, -a.Im); // Унарный минус

        public static Complex operator *(Complex a, Complex b) // Умножение
            => new Complex( // Возвращаем новый Complex
                a.Re * b.Re - a.Im * b.Im, // Re = ac - bd
                a.Re * b.Im + a.Im * b.Re  // Im = ad + bc
            ); // Конец выражения

        public static Complex operator /(Complex a, Complex b) // Деление
        {
            double denom = b.MagnitudeSquared; // denom = c^2 + d^2
            if (denom == 0.0) // Проверяем деление на ноль
                throw new DivideByZeroException("Division by 0 complex number."); // Бросаем исключение
            return new Complex( // Возвращаем результат
                (a.Re * b.Re + a.Im * b.Im) / denom, // Re = (ac + bd) / (c^2 + d^2)
                (a.Im * b.Re - a.Re * b.Im) / denom  // Im = (bc - ad) / (c^2 + d^2)
            ); // Конец вычисления
        }

        public static Complex operator *(Complex z, double k) => new Complex(z.Re * k, z.Im * k); // Умножение на скаляр справа
        public static Complex operator *(double k, Complex z) => z * k; // Умножение на скаляр слева

        public static Complex operator /(Complex z, double k) // Деление на скаляр
        {
            if (k == 0.0) // Проверяем деление на ноль
                throw new DivideByZeroException("Division by 0 scalar."); // Бросаем исключение
            return new Complex(z.Re / k, z.Im / k); // Делим обе части на k
        }

        // ===== Полярная и показательная формы ===== // Раздел форм представления
        public static Complex FromPolar(double r, double phi) // Создание из полярных координат
            => new Complex(r * Math.Cos(phi), r * Math.Sin(phi)); // r(cos(phi) + i sin(phi))

        public (double r, double phi) ToPolar() // Получение (r, phi)
            => (Magnitude, Argument); // Возвращаем модуль и аргумент

        public string ToPolarString(int digits = 4) // Формирование строки в полярной форме
        {
            var (r, phi) = ToPolar(); // Получаем r и phi
            string fmt = "F" + digits; // Формат округления, например F4
            string rs = r.ToString(fmt, CultureInfo.InvariantCulture); // r с округлением
            string phis = phi.ToString(fmt, CultureInfo.InvariantCulture); // phi с округлением
            return $"{rs} * (cos({phis}) + i*sin({phis}))"; // Строка полярной формы
        }

        public string ToExponentialString(int digits = 4) // Формирование строки в показательной форме
        {
            var (r, phi) = ToPolar(); // Получаем r и phi
            string fmt = "F" + digits; // Формат округления
            string rs = r.ToString(fmt, CultureInfo.InvariantCulture); // r с округлением
            string phis = phi.ToString(fmt, CultureInfo.InvariantCulture); // phi с округлением
            return $"{rs} * e^(i*{phis})"; // Строка показательной формы
        }

        // ===== Стандартные комплексные функции ===== // Раздел функций

        public static Complex Exp(Complex z) // exp(z)
        {
            double ex = Math.Exp(z.Re); // e^x
            return new Complex(ex * Math.Cos(z.Im), ex * Math.Sin(z.Im)); // e^x (cos y + i sin y)
        }

        public static Complex Ln(Complex z) // Ln(z) (главная ветвь)
        {
            if (z.MagnitudeSquared == 0.0) // Проверяем z == 0
                throw new ArgumentException("Ln(0) is undefined."); // Ln(0) не определён
            return new Complex(Math.Log(z.Magnitude), z.Argument); // ln|z| + i Arg(z)
        }

        public static Complex Sin(Complex z) // Sin(z)
            => new Complex( // Возвращаем новый Complex
                Math.Sin(z.Re) * Math.Cosh(z.Im), // Re = sin(x)cosh(y)
                Math.Cos(z.Re) * Math.Sinh(z.Im)  // Im = cos(x)sinh(y)
            ); // Конец

        public static Complex Cos(Complex z) // Cos(z)
            => new Complex( // Возвращаем новый Complex
                Math.Cos(z.Re) * Math.Cosh(z.Im),  // Re = cos(x)cosh(y)
                -Math.Sin(z.Re) * Math.Sinh(z.Im)  // Im = -sin(x)sinh(y)
            ); // Конец

        public static Complex Tan(Complex z) => Sin(z) / Cos(z); // Tan(z) = Sin(z)/Cos(z)

        public static Complex Sqrt(Complex z) // Sqrt(z) (главный корень)
        {
            if (z.MagnitudeSquared == 0.0) // Если z == 0
                return Zero; // sqrt(0) = 0
            double r = z.Magnitude; // r = |z|
            double phi = z.Argument; // phi = Arg(z)
            return FromPolar(Math.Sqrt(r), phi / 2.0); // sqrt(r) * e^{i*phi/2}
        }

        // ===== Степени ===== // Раздел степеней
        public Complex Square() => this * this; // z^2
        public Complex Cube() => this * this * this; // z^3

        public Complex Pow(int n) // z^n (целая степень)
        {
            if (n == 0) // Если показатель 0
                return One; // z^0 = 1
            if (n < 0) // Если показатель отрицательный
                return Pow(-n).Reciprocal(); // z^-n = 1/z^n

            Complex result = One; // Результат начинаем с 1
            Complex baseVal = this; // Основание степени
            int p = n; // Копия показателя

            while (p > 0) // Пока показатель > 0
            {
                if ((p & 1) == 1) // Если младший бит равен 1
                    result = result * baseVal; // Домножаем результат
                baseVal = baseVal * baseVal; // Возводим основание в квадрат
                p >>= 1; // Делим показатель на 2 (сдвиг)
            }

            return result; // Возвращаем результат
        }

        public Complex Pow(double y) // z^y (вещественная степень)
        {
            if (MagnitudeSquared == 0.0) // Если z == 0
            {
                if (y > 0) // Если y > 0
                    return Zero; // 0^y = 0
                throw new ArgumentException("0^y is undefined for y <= 0."); // Иначе не определено
            }

            double lnR = Math.Log(Magnitude); // ln|z|
            double phi = Argument; // Arg(z)
            double a = y * lnR; // a = y*ln|z|
            double b = y * phi; // b = y*Arg(z)
            double ea = Math.Exp(a); // exp(a) = |z|^y
            return new Complex(ea * Math.Cos(b), ea * Math.Sin(b)); // |z|^y (cos(b) + i sin(b))
        }

        public Complex Pow(Complex w) // z^w (комплексная степень)
        {
            Complex logz = Ln(this); // log(z)
            Complex wlog = w * logz; // w*log(z)
            return Exp(wlog); // exp(w*log(z))
        }

        // ===== Parse / TryParse ===== // Раздел парсинга

        /// Parse: преобразует строку в комплексное число. // Описание
        /// Поддерживается: "3+4i", "3-4i", "4i", "-i", "3", "0.5+2.1i". // Форматы
        /// Дополнительно: "(3,4)", "(3;4)", "3,4" или "3;4" как пара (Re,Im) (с точкой в дробной части). // Форматы пары
        /// Разделитель дробной части: точка (InvariantCulture). // Важное замечание
        /// </summary> // Конец комментария
        public static Complex Parse(string s) // Метод Parse
        {
            if (TryParse(s, out Complex value)) // Пытаемся распарсить безопасно
                return value; // Если получилось — возвращаем результат
            throw new FormatException("Invalid complex number format."); // Иначе бросаем исключение
        }

        /// <summary>TryParse: безопасный разбор строки, возвращает true/false.</summary> // Комментарий к TryParse
        public static bool TryParse(string? s, out Complex value) // Метод безопасного парсинга
        {
            value = Zero; // Значение по умолчанию

            if (s == null) // Если входная строка null
                return false; // Сообщаем об ошибке

            string t = s.Trim(); // Убираем пробелы по краям

            if (t.Length == 0) // Если строка пустая
                return false; // Сообщаем об ошибке

            t = RemoveSpaces(t); // Убираем все пробелы внутри

            // 1) Попытка распарсить как пару (Re,Im) в форматах "(a,b)" "(a;b)" "a,b" "a;b" // Комментарий
            if (TryParsePair(t, out value)) // Если получилось распарсить как пару
                return true; // Успех

            // 2) Если нет 'i' — это чисто вещественное число // Комментарий
            if (!t.Contains('i')) // Проверяем наличие мнимой единицы
            {
                if (!TryParseDoubleInvariant(t, out double reOnly)) // Пытаемся распарсить double
                    return false; // Если не удалось — ошибка
                value = new Complex(reOnly, 0); // Формируем комплексное число
                return true; // Успех
            }

            // 3) Короткие формы "i", "+i", "-i" // Комментарий
            if (t == "i" || t == "+i") // Проверяем i
            {
                value = new Complex(0, 1); // Присваиваем 0+1i
                return true; // Успех
            }

            if (t == "-i") // Проверяем -i
            {
                value = new Complex(0, -1); // Присваиваем 0-1i
                return true; // Успех
            }

            // 4) Общий случай: должны оканчиваться на 'i' (например "3+4i" или "4i") // Комментарий
            if (!t.EndsWith("i")) // Если 'i' не в конце — формат неверный
                return false; // Сообщаем об ошибке

            string withoutI = t.Substring(0, t.Length - 1); // Убираем завершающий символ 'i'

            // 5) Ищем разделитель между Re и Im: '+' или '-' не в начале (например "3+4" или "3-4") // Комментарий
            int split = FindSplitIndex(withoutI); // Ищем позицию разделения

            if (split == -1) // Если разделителя нет — число чисто мнимое ("4i", "-2.5i")
            {
                if (!TryParseImagPart(withoutI, out double imOnly)) // Парсим коэффициент при i
                    return false; // Если не удалось — ошибка
                value = new Complex(0, imOnly); // Формируем 0 + im*i
                return true; // Успех
            }

            // 6) Если разделитель есть — слева Re, справа Im (со знаком) // Комментарий
            string reStr = withoutI.Substring(0, split); // Вырезаем строку Re
            string imStr = withoutI.Substring(split); // Вырезаем строку Im (включая знак)

            if (!TryParseDoubleInvariant(reStr, out double re)) // Парсим Re
                return false; // Ошибка
            if (!TryParseImagPart(imStr, out double im)) // Парсим Im
                return false; // Ошибка

            value = new Complex(re, im); // Создаём комплексное число
            return true; // Успех
        }

        private static bool TryParsePair(string t, out Complex value) // Парсинг пары (Re,Im)
        {
            value = Zero; // Значение по умолчанию

            string inner = t; // Внутренняя строка, которую будем анализировать

            if (inner.StartsWith("(") && inner.EndsWith(")")) // Если строка в скобках
                inner = inner.Substring(1, inner.Length - 2); // Убираем скобки

            // Поддерживаем разделители ',' и ';' между Re и Im // Комментарий
            int comma = inner.IndexOf(','); // Ищем запятую как разделитель
            int semi = inner.IndexOf(';'); // Ищем точку с запятой как разделитель

            int sep; // Здесь будет индекс разделителя

            if (comma >= 0 && semi >= 0) // Если внезапно есть оба разделителя
                return false; // Считаем формат неоднозначным и возвращаем ошибку

            if (comma >= 0) // Если найден разделитель ','
                sep = comma; // Используем запятую
            else if (semi >= 0) // Если найден разделитель ';'
                sep = semi; // Используем точку с запятой
            else
                return false; // Если нет разделителя — это не пара

            string left = inner.Substring(0, sep); // Левая часть (Re)
            string right = inner.Substring(sep + 1); // Правая часть (Im)

            if (left.Length == 0 || right.Length == 0) // Если какая-то часть пустая
                return false; // Ошибка

            if (!TryParseDoubleInvariant(left, out double re)) // Парсим Re
                return false; // Ошибка
            if (!TryParseDoubleInvariant(right, out double im)) // Парсим Im
                return false; // Ошибка

            value = new Complex(re, im); // Собираем комплексное число
            return true; // Успех
        }

        private static string RemoveSpaces(string s) // Удаление пробелов
        {
            // Быстрый способ убрать пробелы без LINQ // Комментарий
            char[] buffer = new char[s.Length]; // Создаём буфер символов
            int j = 0; // Индекс записи в буфер
            for (int i = 0; i < s.Length; i++) // Проходим по всем символам
            {
                char c = s[i]; // Берём текущий символ
                if (c != ' ') // Если это не пробел
                {
                    buffer[j] = c; // Копируем символ в буфер
                    j++; // Увеличиваем индекс
                }
            }
            return new string(buffer, 0, j); // Возвращаем строку из буфера
        }

        private static int FindSplitIndex(string s) // Поиск места разделения Re и Im
        {
            for (int i = 1; i < s.Length; i++) // Ищем '+' или '-' начиная с 1 (чтобы не спутать знак в начале)
            {
                char c = s[i]; // Берём символ
                if (c == '+' || c == '-') // Если это плюс или минус
                    return i; // Возвращаем индекс разделителя
            }
            return -1; // Не найдено
        }

        private static bool TryParseImagPart(string s, out double im) // Парсинг коэффициента при i
        {
            im = 0.0; // Значение по умолчанию

            if (s == "+") // Если просто "+"
            {
                im = 1.0; // Это означает +1
                return true; // Успех
            }

            if (s == "-") // Если просто "-"
            {
                im = -1.0; // Это означает -1
                return true; // Успех
            }

            if (s.Length == 0) // Если пусто (на всякий случай)
            {
                im = 1.0; // Считаем как 1
                return true; // Успех
            }

            return TryParseDoubleInvariant(s, out im); // Иначе пробуем распарсить число
        }

        private static bool TryParseDoubleInvariant(string s, out double value) // Безопасный парсинг double (с точкой)
        {
            return double.TryParse( // Вызываем TryParse
                s, // Строка
                NumberStyles.Float, // Разрешаем float-форматы
                CultureInfo.InvariantCulture, // Используем инвариантную культуру (точка)
                out value // Выходной параметр
            ); // Возвращаем true/false
        }

        // ===== Служебное: сравнение, вывод, равенство ===== // Раздел служебных методов

        public bool AlmostEquals(Complex other, double eps = 1e-9) // Сравнение с допуском
            => Math.Abs(Re - other.Re) <= eps && Math.Abs(Im - other.Im) <= eps; // Проверяем обе компоненты

        public override string ToString() // Строковое представление a +/- bi
        {
            string reS = Re.ToString(CultureInfo.InvariantCulture); // Преобразуем Re в строку
            string imS = Im.ToString(CultureInfo.InvariantCulture); // Преобразуем Im в строку

            if (Im == 0) // Если мнимая часть 0
                return reS; // Возвращаем только Re

            if (Re == 0) // Если действительная часть 0
                return $"{imS}i"; // Возвращаем только Im*i

            string sign = Im >= 0 ? "+" : "-"; // Определяем знак между частями
            string absImS = Math.Abs(Im).ToString(CultureInfo.InvariantCulture); // Берём модуль Im для красивого вывода
            return $"{reS} {sign} {absImS}i"; // Формируем строку вида "a + bi" или "a - bi"
        }

        public bool Equals(Complex other) => Re.Equals(other.Re) && Im.Equals(other.Im); // Точное равенство
        public override bool Equals(object? obj) => obj is Complex other && Equals(other); // Equals для object
        public override int GetHashCode() => HashCode.Combine(Re, Im); // Хэш-код

        public static bool operator ==(Complex a, Complex b) => a.Equals(b); // Оператор ==
        public static bool operator !=(Complex a, Complex b) => !a.Equals(b); // Оператор !=
    }

    /// <summary>Класс стандартных математических констант.</summary> // Комментарий
    public static class MathConst // Класс констант
    {
        public const double Pi = Math.PI; // Константа π
        public const double E = Math.E; // Константа e
        public const double TwoPi = 2.0 * Math.PI; // Константа 2π
    }
}