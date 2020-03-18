using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using REVUnit.Crlib.Extension;

namespace REVUnit.Crlib.Input
{
    public class Cin
    {
        private string _nextToken;
        public bool IgnoreCase { get; set; } = true;

        public T Get<T>(string hint = null, Func<string, T> parser = null)
        {
            Type t = typeof(T);

            T DefaultParser(string token)
            {
                if (t.IsEnum)
                    return (T) Enum.Parse(t, token, IgnoreCase);
                else if (typeof(IConvertible).IsAssignableFrom(t))
                    return (T) ((IConvertible) token).ToType(t, null);
                else
                    throw new Exception($"Unsupported type {t}.");
            }

            parser ??= DefaultParser;

            bool TryParse(string token, out T result2)
            {
                try
                {
                    result2 = parser(token);
                    return true;
                }
                catch
                {
                    try
                    {
                        result2 = DefaultParser(token);
                    }
                    catch
                    {
                        result2 = default;
                        return false;
                    }

                    result2 = default;
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(hint))
                hint = string.Empty;
            else if (!hint.EndsWith(": ")) hint += ": ";

            T result;
            while (true)
            {
                Console.Write(hint);
                if (!NextToken()) throw new EndOfStreamException();
                if (string.IsNullOrWhiteSpace(_nextToken) || !TryParse(_nextToken, out result))
                {
                    Console.WriteLine($"Expecting a {t.Name} literal, invalid token \"{_nextToken}\".");
                    if (t.IsEnum)
                        Console.WriteLine(
                            $"Available options:{Environment.NewLine}{t.GetEnumValues().Cast<IConvertible>().Select(it => it.ToType(t.GetEnumUnderlyingType())).Select(it => $"[{it}]={t.GetEnumName(it)}").GetLiteral()}");
                    hint = "Enter again: ";
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        public static T Get<T>(Func<string, T> parser)
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
                        XConsole.Backspace();
                    }
                }
            }

            Console.WriteLine();
            return parser(new string(stack.Reverse().ToArray()));
        }

        public bool NextToken()
        {
            var stringBuilder = new StringBuilder();

            while (true)
            {
                int read = Console.Read();
                if (read == -1) return false;
                var readc = (char) read;
                if (Environment.NewLine[0] == readc)
                {
                    if (Environment.NewLine.Length == 2) Console.Read(); // \r Already read, discard \n

                    _nextToken = stringBuilder.ToString();
                    return true;
                }

                stringBuilder.Append(readc);
            }
        }
    }
}