using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using REVUnit.Crlib.WindowsOnly;

namespace REVUnit.Crlib.Extension
{
    public static class XConsole
    {
        static XConsole()
        {
            if (!Native.SetConsoleCtrlHandler(ctrlType =>
            {
                Exiting?.Invoke();
                return true;
            }, true))
            {
                throw new Exception("Unable to set ConsoleCtrlHandler");
            }
        }

        public static IntPtr WindowHandle => Native.GetConsoleWindow();

        public static int[] ReadInts()
        {
            return Console.ReadLine()?.Trim().Split(' ').Select(int.Parse).ToArray();
        }

        public static double[] ReadDoubles()
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
            return (char) Console.Read();
        }

        public static void Write(string text, ConsoleColor color)
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = foregroundColor;
        }

        public static void WriteLine(string text, ConsoleColor color)
        {
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = foregroundColor;
        }

        public static string ReadLine(string hint)
        {
            Console.Write(hint);
            return Console.ReadLine();
        }

        public static string ReadLine(string hint, ConsoleColor inColor)
        {
            Console.Write(hint);
            ConsoleColor foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = inColor;
            string result = Console.ReadLine();
            Console.ForegroundColor = foregroundColor;
            return result;
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

        public static event Action Exiting;
    }
}