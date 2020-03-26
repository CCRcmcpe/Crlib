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
            bool Add);

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

            internal Point Location
            {
                get => new Point(Left, Top);
                set
                {
                    X = value.X;
                    Y = value.Y;
                }
            }

            internal Size Size
            {
                get => new Size(Width, Height);
                set
                {
                    Width = value.Width;
                    Height = value.Height;
                }
            }

            public static Rect operator -(Rect a, Rect b)
            {
                return new Rect(a.Left - b.Left, a.Top - b.Top, a.Right - b.Right, a.Bottom - b.Bottom);
            }

            public static implicit operator Rectangle(Rect r)
            {
                return new Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator Rect(Rectangle r)
            {
                return new Rect(r);
            }

            public static bool operator ==(Rect r1, Rect r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(Rect r1, Rect r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(Rect r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is Rect r) return Equals(r);
                return false;
            }

            public override string ToString()
            {
                return $"{{Left={Left},Top={Top},Right={Right},Bottom={Bottom}}}";
            }

            public int Left;

            public int Top;

            public int Right;

            public int Bottom;
        }
    }
}