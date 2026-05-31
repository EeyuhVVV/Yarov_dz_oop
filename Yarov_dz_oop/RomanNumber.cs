using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yarov_dz_oop;

/// <summary>
/// Класс для представления и работы с римскими числами.
/// Поддерживает арифметические операции, конвертацию между системами счисления,
/// а также перегрузку операторов +, -, *, /, %.
/// </summary>
public class RomanNumber : IComparable<RomanNumber>, IEquatable<RomanNumber>
{
    // Таблицы для перевода: римские символы -> десятичные значения
    private static readonly Dictionary<char, int> RomanToDecimalMap = new()
    {
        { 'I', 1 },
        { 'V', 5 },
        { 'X', 10 },
        { 'L', 50 },
        { 'C', 100 },
        { 'D', 500 },
        { 'M', 1000 }
    };

    // Таблицы для перевода: десятичные значения -> римские символы
    private static readonly (int Value, string Symbol)[] DecimalToRomanTable =
    {
        (1000, "M"),  (900, "CM"), (500, "D"),  (400, "CD"),
        (100,  "C"),  (90,  "XC"), (50,  "L"),  (40,  "XL"),
        (10,   "X"),  (9,   "IX"), (5,   "V"),  (4,   "IV"),
        (1,    "I")
    };

    // Внутреннее хранение значения в десятичной форме
    private int _value;

    /// <summary>
    /// Десятичное значение числа (только чтение).
    /// </summary>
    public int DecimalValue => _value;

    /// <summary>
    /// Римское представление числа (только чтение).
    /// </summary>
    public string RomanValue => ToRomanString(_value);

    /// <summary>
    /// Конструктор из десятичного числа.
    /// </summary>
    /// <param name="decimalValue">Целое положительное число от 1 до 3999.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Если число выходит за диапазон [1; 3999].
    /// </exception>
    public RomanNumber(int decimalValue)
    {
        ValidateRange(decimalValue);
        _value = decimalValue;
    }

    /// <summary>
    /// Конструктор из строки с римским числом.
    /// </summary>
    /// <param name="romanString">Строка в формате римской системы счисления (I, V, X, L, C, D, M).</param>
    /// <exception cref="ArgumentNullException">Если строка null или пустая.</exception>
    /// <exception cref="ArgumentException">Если строка содержит недопустимые символы.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Если результат выходит за диапазон [1; 3999].</exception>
    public RomanNumber(string romanString)
    {
        if (string.IsNullOrWhiteSpace(romanString))
            throw new ArgumentNullException(nameof(romanString), "Римская строка не может быть пустой.");

        _value = FromRomanString(romanString);
        ValidateRange(_value);
    }

    /// <summary>
    /// Конструктор по умолчанию. Значение = 1 (I).
    /// </summary>
    public RomanNumber()
    {
        _value = 1;
    }

    // ===== АРИФМЕТИЧЕСКИЕ МЕТОДЫ =====

    /// <summary>
    /// Сложение двух римских чисел.
    /// </summary>
    public RomanNumber Add(RomanNumber other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other), "Второй операнд не может быть null.");

        int result = _value + other._value;
        ValidateRange(result, "Результат сложения");
        return new RomanNumber(result);
    }

    /// <summary>
    /// Вычитание римского числа.
    /// </summary>
    public RomanNumber Subtract(RomanNumber other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other), "Второй операнд не может быть null.");

        int result = _value - other._value;
        if (result <= 0)
            throw new ArithmeticException(
                $"Результат вычитания ({_value} - {other._value} = {result}) должен быть положительным числом. " +
                "Римская система не поддерживает нулевые и отрицательные значения.");

        ValidateRange(result, "Результат вычитания");
        return new RomanNumber(result);
    }

    /// <summary>
    /// Умножение двух римских чисел.
    /// </summary>
    public RomanNumber Multiply(RomanNumber other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other), "Второй операнд не может быть null.");

        int result = _value * other._value;
        ValidateRange(result, "Результат умножения");
        return new RomanNumber(result);
    }

    /// <summary>
    /// Целочисленное деление.
    /// </summary>
    public RomanNumber Divide(RomanNumber other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other), "Второй операнд не может быть null.");

        if (other._value == 0)
            throw new DivideByZeroException("Деление на ноль невозможно.");

        int result = _value / other._value;
        if (result <= 0)
            throw new ArithmeticException(
                $"Результат целочисленного деления ({_value} / {other._value} = {result}) должен быть положительным числом. " +
                "Римская система не поддерживает нулевые значения.");

        ValidateRange(result, "Результат деления");
        return new RomanNumber(result);
    }

    /// <summary>
    /// Остаток от целочисленного деления.
    /// </summary>
    public RomanNumber Mod(RomanNumber other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other), "Второй операнд не может быть null.");

        if (other._value == 0)
            throw new DivideByZeroException("Деление на ноль невозможно.");

        int result = _value % other._value;
        if (result <= 0)
            throw new ArithmeticException(
                $"Остаток от деления ({_value} % {other._value} = {result}) должен быть положительным числом. " +
                "Римская система не поддерживает нулевые значения.");

        ValidateRange(result, "Результат остатка от деления");
        return new RomanNumber(result);
    }

    /// <summary>
    /// Сброс значения на начальное (I = 1).
    /// </summary>
    public void Reset()
    {
        _value = 1;
    }

    // ===== ПЕРЕГРУЗКА ОПЕРАТОРОВ =====

    public static RomanNumber operator +(RomanNumber a, RomanNumber b) => a.Add(b);
    public static RomanNumber operator -(RomanNumber a, RomanNumber b) => a.Subtract(b);
    public static RomanNumber operator *(RomanNumber a, RomanNumber b) => a.Multiply(b);
    public static RomanNumber operator /(RomanNumber a, RomanNumber b) => a.Divide(b);
    public static RomanNumber operator %(RomanNumber a, RomanNumber b) => a.Mod(b);

    public static bool operator ==(RomanNumber? a, RomanNumber? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a._value == b._value;
    }

    public static bool operator !=(RomanNumber? a, RomanNumber? b) => !(a == b);
    public static bool operator <(RomanNumber a, RomanNumber b) => a._value < b._value;
    public static bool operator >(RomanNumber a, RomanNumber b) => a._value > b._value;
    public static bool operator <=(RomanNumber a, RomanNumber b) => a._value <= b._value;
    public static bool operator >=(RomanNumber a, RomanNumber b) => a._value >= b._value;

    // Явное преобразование: int -> RomanNumber
    public static explicit operator RomanNumber(int value) => new RomanNumber(value);

    // Явное преобразование: RomanNumber -> int
    public static explicit operator int(RomanNumber roman) => roman._value;

    // ===== СТАТИЧЕСКИЕ МЕТОДЫ КОНВЕРТАЦИИ =====

    /// <summary>
    /// Переводит строку с римским числом в десятичное значение.
    /// Алгоритм: проходим справа налево; если текущий символ меньше предыдущего — вычитаем, иначе — прибавляем.
    /// </summary>
    public static int FromRomanString(string roman)
    {
        if (string.IsNullOrWhiteSpace(roman))
            throw new ArgumentNullException(nameof(roman), "Строка не может быть пустой.");

        roman = roman.Trim().ToUpper();

        // Валидация символов
        foreach (char c in roman)
        {
            if (!RomanToDecimalMap.ContainsKey(c))
                throw new ArgumentException(
                    $"Недопустимый символ '{c}' в римском числе \"{roman}\". " +
                    "Допустимые символы: I, V, X, L, C, D, M.");
        }

        int result = 0;
        int prevValue = 0;

        // Проход справа налево
        for (int i = roman.Length - 1; i >= 0; i--)
        {
            int currentValue = RomanToDecimalMap[roman[i]];

            if (currentValue < prevValue)
            {
                result -= currentValue; // Правило вычитания (IV = 4, IX = 9, и т.д.)
            }
            else
            {
                result += currentValue;
            }

            prevValue = currentValue;
        }

        return result;
    }

    /// <summary>
    /// Переводит десятичное число в строку римского числа.
    /// Использует «жадный» алгоритм с таблицей пар (значение, символ).
    /// </summary>
    public static string ToRomanString(int number)
    {
        if (number <= 0 || number > 3999)
            throw new ArgumentOutOfRangeException(nameof(number),
                $"Число {number} не может быть представлено в римской системе. Допустимый диапазон: 1–3999.");

        var sb = new StringBuilder();

        foreach (var (value, symbol) in DecimalToRomanTable)
        {
            while (number >= value)
            {
                sb.Append(symbol);
                number -= value;
            }
        }

        return sb.ToString();
    }

    // ===== СЛУЖЕБНЫЕ МЕТОДЫ =====

    /// <summary>
    /// Валидация диапазона [1; 3999].
    /// </summary>
    private static void ValidateRange(int value, string context = "Число")
    {
        if (value <= 0 || value > 3999)
            throw new ArgumentOutOfRangeException(nameof(value),
                $"{context} = {value}. Допустимый диапазон: 1–3999.");
    }

    // ===== ПЕРЕОПРЕДЕЛЕНИЕ СТАНДАРТНЫХ МЕТОДОВ =====

    public override string ToString() => $"{RomanValue} ({_value})";

    public override bool Equals(object? obj) => obj is RomanNumber other && _value == other._value;

    public bool Equals(RomanNumber? other) => other is not null && _value == other._value;

    public override int GetHashCode() => _value.GetHashCode();

    public int CompareTo(RomanNumber? other)
    {
        if (other is null) return 1;
        return _value.CompareTo(other._value);
    }
}
