using System;
using System.IO;
using System.Linq;
using REVUnit.Crlib.Properties;

namespace REVUnit.Crlib.Extension
{
    public static class XConsole
    {
        public static void AnyKey()
        {
            Console.WriteLine(Resources.XConsole_AnyKey);
            Console.ReadKey(true);
        }

        public static void AnyKey(string message)
        {
            Console.WriteLine(message + Resources.XConsole_AnyKey_WithPrefix);
            Console.ReadKey(true);
        }

        public static void Backspace()
        {
            // ReSharper disable once LocalizableElement
            Console.Write("\b\0\b");
        }

        public static void FormatWrite(params string[] texts)
        {
            int num = texts.Length - 1;
            for (var i = 0; i < num; i++) Console.Write(texts[i]);
            Console.WriteLine(texts[num]);
        }

        public static char Read()
        {
            int i = Console.Read();
            if (i < char.MinValue) throw new IOException("Unexcepted EOF");
            return (char) i;
        }

        public static double[]? ReadDoublesLine() =>
            Console.ReadLine()?.Trim().Split(' ').Select(double.Parse).ToArray();

        public static int[]? ReadIntsLine() => Console.ReadLine()?.Trim().Split(' ').Select(int.Parse).ToArray();

        public static string ReadLine(string hint)
        {
            Console.Write(hint);
            return Console.ReadLine() ?? throw new IOException("Unexpected EOF");
        }
#if WINDOWS
        static XConsole()
        {
            if (!Native.SetConsoleCtrlHandler(ctrlType =>
            {
                Exiting?.Invoke();
                return true;
            }, true))
                throw new Exception("Unable to set ConsoleCtrlHandler");
        }

        public static IntPtr WindowHandle => Native.GetConsoleWindow();

        public static event Action? Exiting;
#endif
    }
}