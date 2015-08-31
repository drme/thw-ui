using System;

namespace ThW.UI.Utils
{
    public enum Key : int
    {
        None = 0,
        ShiftLeft = 1 << 1,
        ShiftRight = 1 << 2,
        Alt = 1 << 3,
        Ctrl = 1 << 4,
        CapsLock = 1 << 5,
        NumLock = 1 << 6,
        ScrollLock = 1 << 7,
        Left = 1 << 8,
        Right = 1 << 9,
        Up = 1 << 10,
        Down = 1 << 11,
        PgUp = 1 << 12,
        PgDn = 1 << 13,
        Home = 1 << 14,
        End = 1 << 15,
        Delete = 1 << 16,
        Backspace = 1 << 17,
        Enter = 1 << 18,
        Tab = 1 << 19,
        Space = 1 << 20,
        Escape = 1 << 21,
        Back = 1 << 22,
		Insert = 1 << 23,
		NumPad8 = 1 << 24,
		NumPad2 = 1 << 25,
		NumPad4 = 1 << 26,
		NumPad6 = 1 << 27,
		NumPad9 = 1 << 28,
		NumPad3 = 1 << 29,
		NumPad7 = 1 << 30,
		NumPad1 = 1 << 31
    }
}
