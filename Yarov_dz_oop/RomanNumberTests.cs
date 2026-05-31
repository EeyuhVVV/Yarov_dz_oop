using System;

namespace Yarov_dz_oop;

/// <summary>
/// Класс для тестирования функциональности RomanNumber.
/// Запускается отдельно для проверки корректности всех операций.
/// </summary>
public static class RomanNumberTests
{
    private static int _passed = 0;
    private static int _failed = 0;

    /// <summary>
    /// Запуск всех тестов и вывод результатов.
    /// </summary>
    public static void RunAll()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Тестирование класса RomanNumber ===\n");

        TestConversionToRoman();
        TestConversionFromRoman();
        TestAddition();
        TestSubtraction();
        TestMultiplication();
        TestDivision();
        TestMod();
        TestOperatorOverloads();
        TestComparison();
        TestExceptions();
        TestReset();

        Console.WriteLine($"\n=== Итого: {_passed} пройдено, {_failed} провалено ===");
    }

    // ===== ТЕСТЫ КОНВЕРТАЦИИ =====

    private static void TestConversionToRoman()
    {
        Console.WriteLine("--- Конвертация: десятичное → римское ---");
        AssertEqual(RomanNumber.ToRomanString(1), "I", "1 → I");
        AssertEqual(RomanNumber.ToRomanString(4), "IV", "4 → IV");
        AssertEqual(RomanNumber.ToRomanString(9), "IX", "9 → IX");
        AssertEqual(RomanNumber.ToRomanString(14), "XIV", "14 → XIV");
        AssertEqual(RomanNumber.ToRomanString(42), "XLII", "42 → XLII");
        AssertEqual(RomanNumber.ToRomanString(99), "XCIX", "99 → XCIX");
        AssertEqual(RomanNumber.ToRomanString(399), "CCCXCIX", "399 → CCCXCIX");
        AssertEqual(RomanNumber.ToRomanString(944), "CMXLIV", "944 → CMXLIV");
        AssertEqual(RomanNumber.ToRomanString(1994), "MCMXCIV", "1994 → MCMXCIV");
        AssertEqual(RomanNumber.ToRomanString(3999), "MMMCMXCIX", "3999 → MMMCMXCIX");
        Console.WriteLine();
    }

    private static void TestConversionFromRoman()
    {
        Console.WriteLine("--- Конвертация: римское → десятичное ---");
        AssertEqual(RomanNumber.FromRomanString("I"), 1, "I → 1");
        AssertEqual(RomanNumber.FromRomanString("IV"), 4, "IV → 4");
        AssertEqual(RomanNumber.FromRomanString("IX"), 9, "IX → 9");
        AssertEqual(RomanNumber.FromRomanString("XIV"), 14, "XIV → 14");
        AssertEqual(RomanNumber.FromRomanString("XLII"), 42, "XLII → 42");
        AssertEqual(RomanNumber.FromRomanString("XCIX"), 99, "XCIX → 99");
        AssertEqual(RomanNumber.FromRomanString("MCMXCIV"), 1994, "MCMXCIV → 1994");
        AssertEqual(RomanNumber.FromRomanString("MMMCMXCIX"), 3999, "MMMCMXCIX → 3999");
        Console.WriteLine();
    }

    // ===== ТЕСТЫ АРИФМЕТИКИ =====

    private static void TestAddition()
    {
        Console.WriteLine("--- Сложение ---");
        var a = new RomanNumber("X");   // 10
        var b = new RomanNumber("V");   // 5
        var result = a + b;
        AssertEqual(result.DecimalValue, 15, "X + V = XV (15)");
        AssertEqual(result.RomanValue, "XV", "X + V = XV (строка)");

        var c = new RomanNumber(100);
        var d = new RomanNumber(250);
        result = c + d;
        AssertEqual(result.DecimalValue, 350, "C + CCL = CCCL (350)");
        Console.WriteLine();
    }

    private static void TestSubtraction()
    {
        Console.WriteLine("--- Вычитание ---");
        var a = new RomanNumber("XX");  // 20
        var b = new RomanNumber("V");   // 5
        var result = a - b;
        AssertEqual(result.DecimalValue, 15, "XX - V = XV (15)");

        var c = new RomanNumber(1000);
        var d = new RomanNumber(1);
        result = c - d;
        AssertEqual(result.DecimalValue, 999, "M - I = CMXCIX (999)");
        Console.WriteLine();
    }

    private static void TestMultiplication()
    {
        Console.WriteLine("--- Умножение ---");
        var a = new RomanNumber("X");   // 10
        var b = new RomanNumber("III"); // 3
        var result = a * b;
        AssertEqual(result.DecimalValue, 30, "X × III = XXX (30)");

        var c = new RomanNumber(50);
        var d = new RomanNumber(2);
        result = c * d;
        AssertEqual(result.DecimalValue, 100, "L × II = C (100)");
        Console.WriteLine();
    }

    private static void TestDivision()
    {
        Console.WriteLine("--- Целочисленное деление ---");
        var a = new RomanNumber("XX");  // 20
        var b = new RomanNumber("IV");  // 4
        var result = a / b;
        AssertEqual(result.DecimalValue, 5, "XX ÷ IV = V (5)");

        var c = new RomanNumber(100);
        var d = new RomanNumber(10);
        result = c / d;
        AssertEqual(result.DecimalValue, 10, "C ÷ X = X (10)");
        Console.WriteLine();
    }

    private static void TestMod()
    {
        Console.WriteLine("--- Остаток от деления ---");
        var a = new RomanNumber("XVII"); // 17
        var b = new RomanNumber("V");    // 5
        var result = a % b;
        AssertEqual(result.DecimalValue, 2, "XVII % V = II (2)");

        var c = new RomanNumber(23);
        var d = new RomanNumber(7);
        result = c % d;
        AssertEqual(result.DecimalValue, 2, "XXIII % VII = II (2)");
        Console.WriteLine();
    }

    private static void TestOperatorOverloads()
    {
        Console.WriteLine("--- Перегрузка операторов ---");
        var a = new RomanNumber(10);
        var b = new RomanNumber(10);
        AssertTrue(a == b, "X == X → true");
        AssertTrue(a != new RomanNumber(5), "X != V → true");
        Console.WriteLine();
    }

    private static void TestComparison()
    {
        Console.WriteLine("--- Сравнение ---");
        var a = new RomanNumber(10);
        var b = new RomanNumber(20);
        AssertTrue(a < b, "X < XX → true");
        AssertTrue(b > a, "XX > X → true");
        AssertTrue(a <= new RomanNumber(10), "X <= X → true");
        AssertTrue(b >= new RomanNumber(20), "XX >= XX → true");
        Console.WriteLine();
    }

    // ===== ТЕСТЫ ИСКЛЮЧЕНИЙ =====

    private static void TestExceptions()
    {
        Console.WriteLine("--- Обработка исключений ---");

        // Выход за диапазон
        AssertThrows<ArgumentOutOfRangeException>(() => new RomanNumber(0), "RomanNumber(0) → исключение");
        AssertThrows<ArgumentOutOfRangeException>(() => new RomanNumber(-5), "RomanNumber(-5) → исключение");
        AssertThrows<ArgumentOutOfRangeException>(() => new RomanNumber(4000), "RomanNumber(4000) → исключение");

        // Пустая строка
        AssertThrows<ArgumentNullException>(() => new RomanNumber(""), "RomanNumber(\"\") → исключение");

        // Недопустимый символ
        AssertThrows<ArgumentException>(() => new RomanNumber("ABC"), "RomanNumber(\"ABC\") → исключение");

        // Вычитание с отрицательным результатом
        AssertThrows<ArithmeticException>(() => new RomanNumber(5) - new RomanNumber(10), "V - X → исключение");

        // Деление на ноль (невозможно в римской системе, но проверяем)
        AssertThrows<ArithmeticException>(() => new RomanNumber(10) / new RomanNumber(20), "X / XX = 0 → исключение");

        // Результат умножения > 3999
        AssertThrows<ArgumentOutOfRangeException>(() => new RomanNumber(2000) * new RomanNumber(3), "MM × III → исключение (>3999)");

        Console.WriteLine();
    }

    private static void TestReset()
    {
        Console.WriteLine("--- Сброс ---");
        var a = new RomanNumber(500);
        a.Reset();
        AssertEqual(a.DecimalValue, 1, "Reset() → значение = 1");
        AssertEqual(a.RomanValue, "I", "Reset() → строка = I");
        Console.WriteLine();
    }

    // ===== ASSERTION HELPERS =====

    private static void AssertEqual<T>(T actual, T expected, string testName)
    {
        if (Equals(actual, expected))
        {
            Console.WriteLine($"  \u2705 {testName}");
            _passed++;
        }
        else
        {
            Console.WriteLine($"  \u274c {testName} \u2014 \u043e\u0436\u0438\u0434\u0430\u043b\u043e\u0441\u044c: {expected}, \u043f\u043e\u043b\u0443\u0447\u0435\u043d\u043e: {actual}");
            _failed++;
        }
    }

    private static void AssertTrue(bool condition, string testName)
    {
        if (condition)
        {
            Console.WriteLine($"  \u2705 {testName}");
            _passed++;
        }
        else
        {
            Console.WriteLine($"  \u274c {testName} \u2014 \u0443\u0441\u043b\u043e\u0432\u0438\u0435 \u043b\u043e\u0436\u043d\u043e");
            _failed++;
        }
    }

    private static void AssertThrows<TException>(Action action, string testName) where TException : Exception
    {
        try
        {
            action();
            Console.WriteLine($"  \u274c {testName} \u2014 \u0438\u0441\u043a\u043b\u044e\u0447\u0435\u043d\u0438\u0435 \u041d\u0415 \u0432\u044b\u0431\u0440\u043e\u0448\u0435\u043d\u043e");
            _failed++;
        }
        catch (TException)
        {
            Console.WriteLine($"  \u2705 {testName}");
            _passed++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  \u274c {testName} \u2014 \u043e\u0436\u0438\u0434\u0430\u043b\u043e\u0441\u044c {typeof(TException).Name}, \u043f\u043e\u043b\u0443\u0447\u0435\u043d\u043e {ex.GetType().Name}: {ex.Message}");
            _failed++;
        }
    }
}
