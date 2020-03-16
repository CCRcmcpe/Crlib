using System;
using System.Collections.Generic;
using System.Linq;
using REVUnit.Crlib.WindowsOnly;

namespace REVUnit.Crlib.Extension
{
    public static class XConsole
    {
        static XConsole()
        {
            Native.SetConsoleCtrlHandler(delegate(int sig)
            {
                if (sig != Native.CTRL_BREAK_EVENT) ConsoleExit?.Invoke();
                return false;
            }, false);
        }

        public static IntPtr Handle => Native.GetConsoleWindow();

        public static int[] ReadInts()
        {
            return Console.ReadLine().Trim().Split(' ').Select(int.Parse).ToArray();
        }

        public static double[] ReadDoubles()
        {
            return Console.ReadLine().Trim().Split(' ').Select(double.Parse).ToArray();
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

        private static void Backspace()
        {
            Console.Write("\b\0\b");
        }

        public static T ReadLine<T>(Func<string, T> parser)
        {
            var stack = new Stack<char>();
            while (true)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                char keyChar = consoleKeyInfo.KeyChar;
                if (!char.IsControl(keyChar))
                {
                    stack.Push(keyChar);
                    try
                    {
                        parser(new string(stack.Reverse().ToArray()));
                        Console.Write(keyChar);
                    }
                    catch (FormatException)
                    {
                        stack.Pop();
                    }
                }
                else
                {
                    if (consoleKeyInfo.Key == ConsoleKey.Enter) break;
                    if (consoleKeyInfo.Key == ConsoleKey.Backspace && stack.Count > 0)
                    {
                        stack.Pop();
                        Backspace();
                    }
                }
            }

            Console.WriteLine();
            return parser(new string(stack.Reverse().ToArray()));
        }

        public static event Action ConsoleExit;

        internal delegate bool ConsoleExitHandler(int sig);
    }
}