using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace REVUnit.Crlib.WindowsOnly
{
    public delegate bool ConsoleCtrlHandler(uint ctrlType);

    internal static class Native
    {
        public const int WmHotkey = 786;

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandler HandlerRoutine,
            bool add);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hWnd, int dwAttribute, out Rect lpRect, int cbAttribute);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, HotKey.KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        internal struct Rect
        {
            internal Rect(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            internal Rect(Rectangle r)
            {
                this = new Rect(r.Left, r.Top, r.Right, r.Bottom);
            }

            internal int X
            {
                get => Left;
                set
                {
                    Right -= Left - value;
                    Left = value;
                }
            }

            internal int Y
            {
                get => Top;
                set
                {
                    Bottom -= Top - value;
                    Top = value;
                }
            }

            internal int Height
            {
                get => Bottom - Top;
                set => Bottom = value + Top;
            }

            internal int Width
            {
                get => Right - Left;
                set => Right = value + Left;
            }

            public bool Equals(Rect r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public int Left;

            public int Top;

            public int Right;

            public int Bottom;
        }
    }
}