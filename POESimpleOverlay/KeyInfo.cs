using GlobalHotKeys.Native.Types;

class KeyInfo
{
    public VirtualKeyCode Key { get; set; }
    public Modifiers Modifiers { get; set; }
    public KeyInfo(ConsoleKeyInfo consoleKeyInfo)
    {
        Key = (VirtualKeyCode)consoleKeyInfo.Key.GetHashCode();

        var consoleKeyInfoHash = consoleKeyInfo.Modifiers.GetHashCode();
        switch (consoleKeyInfoHash)
        {
            case 0: Modifiers = Modifiers.NoRepeat; break;
            case 1: Modifiers = Modifiers.Alt; break;
            case 2: Modifiers = Modifiers.Shift; break;
            case 4: Modifiers = Modifiers.Control; break;
        }
    }
    static public KeyInfo ReadKey()
    {
        var info = Console.ReadKey(false);
        return new KeyInfo(info);
    }
}
