using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using REVUnit.Crlib.Extension;
using REVUnit.Crlib.Properties;

namespace REVUnit.Crlib.Input
{
    public class Cin
    {
        private string? _nextToken;
        public bool IgnoreCase { get; set; } = true;

        public T Get<T>(string? hint = null) where T : IConvertible
        {
            if (string.IsNullOrWhiteSpace(hint))
                hint = string.Empty;
            else if (!hint.EndsWith(": ")) hint += ": ";

            Type t = typeof(T);
            T result;

            while (true)
            {
                if (t.IsEnum)
                    Console.WriteLine(
                        t.GetEnumValues().Cast<IConvertible>().Select(it => it!.ToType(t.GetEnumUnderlyingType()))
                            .Select(it => $"[{it}]={t.GetEnumName(it)}").GetLiteral());
                Console.Write(hint);
                if (!NextToken()) throw new EndOfStreamException();
                if (string.IsNullOrWhiteSpace(_nextToken) || !TryParse(_nextToken, out result))
                {
                    Console.WriteLine(Resources.Cin_InvalidToken, t.Name, _nextToken);
                    hint = Resources.Cin_EnterAgainHint;
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
            if (parser == null) throw new ArgumentNullException(nameof(parser));
            var queue = new Queue<char>();
            while (true)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                char keyChar = consoleKeyInfo.KeyChar;
                if (!char.IsControl(keyChar))
                {
                    queue.Enqueue(keyChar);
                    try
                    {
                        parser(new string(queue.ToArray()));
                        Console.Write(keyChar);
                    }
                    catch (FormatException)
                    {
                        queue.Dequeue();
                    }
                }
                else
                {
                    if (consoleKeyInfo.Key == ConsoleKey.Enter) break;
                    if (consoleKeyInfo.Key != ConsoleKey.Backspace || queue.Count <= 0) continue;
                    queue.Dequeue();
                    XConsole.Backspace();
                }
            }

            Console.WriteLine();
            return parser(new string(queue.ToArray()));
        }

        private bool NextToken()
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

        private bool TryParse<T>(string value, [MaybeNullWhen(false)] out T result) where T : IConvertible
        {
            try
            {
                Type t = typeof(T);
                if (t.IsEnum)
                {
                    var ret = (T) Enum.Parse(t, value, IgnoreCase);
                    if (!Enum.IsDefined(t, ret)) throw new Exception(string.Format(Resources.Cin_InputOutOfRange, t));
                    result = ret;
                }
                else
                {
                    result = (T) value.ToType(t);
                }

                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}