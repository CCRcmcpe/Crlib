using System;
using System.IO;
using System.Linq;

namespace REVUnit.Crlib.Extension
{
    public static class XConsole
    {
        static XConsole()
        {
#if WINDOWS
            if (!Native.SetConsoleCtrlHandler(ctrlType =>
            {
                Exiting?.Invoke();
                return true;
            }, true))
                throw new Exception("Unable to set ConsoleCtrlHandler");
#endif
        }

#if WINDOWS
        public static IntPtr WindowHandle => Native.GetConsoleWindow();
#endif
        public static int[]? ReadIntsLine()
        {
            return Console.ReadLine()?.Trim().Split(' ').Select(int.Parse).ToArray();
        }

        public static double[]? ReadDoublesLine()
        {
            return Console.ReadLine()?.Trim().Split(' ').Select(double.Parse).ToArray();
        }

        public static void AnyKey(string message)
        {
            Console.Write(message + "，请按任意键继续. . .");
            Console.ReadKey(true);
        }

        public static char Read()
        {
            int i = Console.Read();
            if (i < char.MinValue) throw new IOException("Unexcepted EOF");
            return (char) i;
        }

        public static string ReadLine(string hint)
        {
            Console.Write(hint);
            return Console.ReadLine() ?? throw new IOException("Unexpected EOF");
        }

        public static void FormatWrite(params string[] texts)
        {
            int num = texts.Length - 1;
            for (var i = 0; i < num; i++) Console.Write(texts[i]);
            Console.WriteLine(texts[num]);
        }

        public static void Backspace()
        {
            Console.Write("\b\0\b");
        }
#if WINDOWS
        public static event Action? Exiting;
#endif
    }
}