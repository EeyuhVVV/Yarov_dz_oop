namespace Yarov_dz_oop;

static class Program
{
    /// <summary>
    ///  Точка входа приложения — калькулятор римских чисел.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
