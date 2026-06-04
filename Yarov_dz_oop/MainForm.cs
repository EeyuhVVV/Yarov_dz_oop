#nullable disable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yarov_dz_oop;

/// <summary>
/// Главная форма калькулятора римских чисел.
/// Содержит поля ввода, кнопки операций, поле результата и историю.
/// </summary>
public partial class MainForm : Form
{
    // === UI-элементы ===
    private TextBox txtNumber1;
    private TextBox txtNumber2;
    private TextBox txtResult;
    private ListBox lstHistory;

    private Button btnAdd;
    private Button btnSubtract;
    private Button btnMultiply;
    private Button btnDivide;
    private Button btnMod;
    private Button btnToRoman;
    private Button btnToDecimal;
    private Button btnReset;
    private Button btnRunTests;

    private Label lblNumber1;
    private Label lblNumber2;
    private Label lblResult;
    private Label lblHistory;

    public MainForm()
    {
        InitializeComponent();
        SetupUI();
    }

    /// <summary>
    /// Программная инициализация всех элементов интерфейса.
    /// </summary>
    private void SetupUI()
    {
        this.Text = "Калькулятор римских чисел — Яров И.М., БИВТ-25-2";
        this.Size = new Size(620, 520);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Font = new Font("Segoe UI", 10F);
        this.MinimumSize = new Size(620, 520);

        // === Метки ===
        lblNumber1 = new Label { Text = "Число 1:", Location = new Point(20, 20), AutoSize = true };
        lblNumber2 = new Label { Text = "Число 2:", Location = new Point(20, 60), AutoSize = true };
        lblResult = new Label { Text = "Результат:", Location = new Point(20, 200), AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
        lblHistory = new Label { Text = "История операций:", Location = new Point(20, 270), AutoSize = true };

        // === Поля ввода ===
        txtNumber1 = new TextBox { Location = new Point(120, 17), Size = new Size(200, 28), PlaceholderText = "Например: XIV или 14" };
        txtNumber2 = new TextBox { Location = new Point(120, 57), Size = new Size(200, 28), PlaceholderText = "Например: V или 5" };
        txtResult = new TextBox { Location = new Point(120, 197), Size = new Size(460, 28), ReadOnly = true, BackColor = Color.FromArgb(240, 248, 255), Font = new Font("Segoe UI", 11F, FontStyle.Bold) };

        // === Кнопки арифметики ===
        int btnY = 100;
        int btnW = 60;
        int btnH = 35;
        int gap = 8;

        btnAdd = new Button { Text = "+", Location = new Point(20, btnY), Size = new Size(btnW, btnH) };
        btnSubtract = new Button { Text = "−", Location = new Point(20 + (btnW + gap), btnY), Size = new Size(btnW, btnH) };
        btnMultiply = new Button { Text = "×", Location = new Point(20 + 2 * (btnW + gap), btnY), Size = new Size(btnW, btnH) };
        btnDivide = new Button { Text = "÷", Location = new Point(20 + 3 * (btnW + gap), btnY), Size = new Size(btnW, btnH) };
        btnMod = new Button { Text = "%", Location = new Point(20 + 4 * (btnW + gap), btnY), Size = new Size(btnW, btnH) };

        // === Кнопки конвертации и управления ===
        int btnY2 = 148;
        btnToRoman = new Button { Text = "Дес → Рим", Location = new Point(20, btnY2), Size = new Size(120, btnH) };
        btnToDecimal = new Button { Text = "Рим → Дес", Location = new Point(148, btnY2), Size = new Size(120, btnH) };
        btnReset = new Button { Text = "Сброс", Location = new Point(276, btnY2), Size = new Size(100, btnH) };
        btnRunTests = new Button { Text = "Тесты", Location = new Point(384, btnY2), Size = new Size(100, btnH), ForeColor = Color.DarkGreen };

        // === История ===
        lstHistory = new ListBox { Location = new Point(20, 295), Size = new Size(560, 170), Font = new Font("Consolas", 9F) };

        // === Обработчики ===
        btnAdd.Click += (s, e) => DoOperation("+");
        btnSubtract.Click += (s, e) => DoOperation("-");
        btnMultiply.Click += (s, e) => DoOperation("*");
        btnDivide.Click += (s, e) => DoOperation("/");
        btnMod.Click += (s, e) => DoOperation("%");
        btnToRoman.Click += (s, e) => ConvertToRoman();
        btnToDecimal.Click += (s, e) => ConvertToDecimal();
        btnReset.Click += (s, e) => ResetResult();
        btnRunTests.Click += (s, e) => RunTests();

        // === Добавление на форму ===
        this.Controls.AddRange(new Control[]
        {
            lblNumber1, lblNumber2, lblResult, lblHistory,
            txtNumber1, txtNumber2, txtResult,
            btnAdd, btnSubtract, btnMultiply, btnDivide, btnMod,
            btnToRoman, btnToDecimal, btnReset, btnRunTests,
            lstHistory
        });
    }

    /// <summary>
    /// Парсинг ввода: принимает и римскую строку, и десятичное число.
    /// </summary>
    private RomanNumber ParseInput(string input)
    {
        input = input.Trim();
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Поле ввода пустое.");

        if (int.TryParse(input, out int dec))
            return new RomanNumber(dec);

        return new RomanNumber(input);
    }

    /// <summary>
    /// Выполнение арифметической операции.
    /// </summary>
    private void DoOperation(string op)
    {
        try
        {
            RomanNumber a = ParseInput(txtNumber1.Text);
            RomanNumber b = ParseInput(txtNumber2.Text);
            RomanNumber result;

            switch (op)
            {
                case "+": result = a + b; break;
                case "-": result = a - b; break;
                case "*": result = a * b; break;
                case "/": result = a / b; break;
                case "%": result = a % b; break;
                default: return;
            }

            txtResult.Text = result.ToString();
            string entry = $"{a} {op} {b} = {result}";
            lstHistory.Items.Insert(0, entry);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Конвертация десятичного числа в римское.
    /// </summary>
    private void ConvertToRoman()
    {
        try
        {
            string input = txtNumber1.Text.Trim();
            if (!int.TryParse(input, out int dec))
                throw new ArgumentException("Введите десятичное число в поле 'Число 1'.");

            string roman = RomanNumber.ToRomanString(dec);
            txtResult.Text = $"{roman} ({dec})";
            lstHistory.Items.Insert(0, $"Конвертация: {dec} → {roman}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Конвертация римского числа в десятичное.
    /// </summary>
    private void ConvertToDecimal()
    {
        try
        {
            string input = txtNumber1.Text.Trim();
            int dec = RomanNumber.FromRomanString(input);
            txtResult.Text = $"{dec} ({input.ToUpper()})";
            lstHistory.Items.Insert(0, $"Конвертация: {input.ToUpper()} → {dec}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// Сброс результата.
    /// </summary>
    private void ResetResult()
    {
        txtResult.Text = "";
        lstHistory.Items.Insert(0, "--- Сброс ---");
    }

    /// <summary>
    /// Запуск тестов в консоли.
    /// </summary>
    private void RunTests()
    {
        RomanNumberTests.RunAll();
        MessageBox.Show("Тесты выполнены! Результаты выведены в консоль (Output).",
            "Тесты", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
