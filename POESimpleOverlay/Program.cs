using POESimpleOverlay;

internal partial class Program
{

    private static void Main(string[] args)
    {
        Console.TreatControlCAsInput = false;
        AppRun();
    }
    private static void AppRun()
    {        
        GameOverlay.TimerService.EnableHighPrecisionTimers();
        ConsoleHelper.PrintInfo();
        ConsoleHelper.DefaultHotKeyRegistration();

        while (true)
        {
            Console.Write("Введите команду/файл: ");
            var input = Console.ReadLine();
            ConsoleHelper.Handle(input);
        }

    }

}