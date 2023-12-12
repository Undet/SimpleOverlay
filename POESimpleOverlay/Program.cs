using GlobalHotKeys;
using GlobalHotKeys.Native.Types;
using POESimpleOverlay;
using POESimpleOverlay.DataModels;
using POESimpleOverlay.Overlay;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        AppRun();
    }


    private static void AppRun()
    {        
        GameOverlay.TimerService.EnableHighPrecisionTimers();

        ConsoleHelper.PrintInfo();

        void HotKeyPressed(HotKey hotKey)
        {
            if (hotKey.Key == VirtualKeyCode.KEY_Z && hotKey.Modifiers == Modifiers.Control && ConsoleHelper.simpleTextOverlay != null)
            {
                ConsoleHelper.simpleTextOverlay.PreviousString();
            }
            else if (hotKey.Key == VirtualKeyCode.KEY_Z && ConsoleHelper.simpleTextOverlay != null)
            {
                ConsoleHelper.simpleTextOverlay.NextString();
            }

            if (hotKey.Key == VirtualKeyCode.KEY_R && hotKey.Modifiers == Modifiers.Control)
            {
                ConsoleHelper.overlayWrappers.ForEach((x) => x.Overlay.ChangeVisability());
            }
        };
        using var hotKeyManager = new HotKeyManager();
        using var subscription = hotKeyManager.HotKeyPressed.Subscribe(HotKeyPressed);
        using var up = hotKeyManager.Register(VirtualKeyCode.KEY_Z, Modifiers.NoRepeat);
        using var down = hotKeyManager.Register(VirtualKeyCode.KEY_Z, Modifiers.Control);
        using var restart = hotKeyManager.Register(VirtualKeyCode.KEY_R, Modifiers.Control);

        while (true)
        {
            Console.Write("Введите команду/файл: ");
            var input = Console.ReadLine();
            ConsoleHelper.Handle(input);
            
        }

    }
}