using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using REVUnit.Crlib.Extension;
using REVUnit.Crlib.Properties;

namespace REVUnit.Crlib.Input
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string? errorToken)
        {
            ErrorToken = errorToken;
        }

        public string? ErrorToken { get; set; }
        public override string Message => string.Format(Resources.InvalidInputException_Message, ErrorToken);
        public override IDictionary Data => new ListDictionary {{"ErrorToken", ErrorToken}};
    }

    public class Cin : TextScanner
    {
        public Cin() : base(Console.In, Environment.NewLine)
        {
        }

        public bool WriteEnumDescription { get; set; }
        public bool ThrowOnInvalidInput { get; set; }

        public override T Get<T>()
        {
            return Get<T>(null, TryParse);
        }

        public T Get<T>(string? hint) where T : IConvertible
        {
            return Get<T>(hint, TryParse);
        }

        public T Get<T>(string? hint, Func<string, T> parser)
        {
            return Get<T>(hint, new TryParser<string, T>(parser).TryParse);
        }

        public T Get<T>(string? hint, TryParser<string, T>.Agent tryParser)
        {
            bool interactive = Environment.UserInteractive;
            if (string.IsNullOrWhiteSpace(hint))
                hint = string.Empty;
            else if (!hint.EndsWith(": ")) hint += ": ";

            Type type = typeof(T);
            if (type.IsEnum && interactive && WriteEnumDescription) Console.WriteLine(GetEnumDescription(type));

            while (true)
            {
                if (interactive) Console.Write(hint);
                string? token = NextToken();
                if (!tryParser(token!, out T result))
                {
                    if (ThrowOnInvalidInput || !interactive) throw new InvalidInputException(token);
                    Console.WriteLine(Resources.Cin_InvalidInput, token);
                    hint = Resources.Cin_EnterAgainHint;
                }
                else
                {
                    return result;
                }
            }
        }

        private static string GetEnumDescription(Type enumType)
        {
            return enumType.GetEnumValues().Cast<IConvertible>()
                .Select(it => it!.ToType(enumType.GetEnumUnderlyingType()))
                .Select(it => $"[{it}]={enumType.GetEnumName(it)}").GetLiteral();
        }

        private bool TryParse<T>(string value, [MaybeNullWhen(false)] out T result) where T : IConvertible
        {
            try
            {
                Type type = typeof(T);
                if (type.IsEnum)
                {
                    var parsed = (T) Enum.Parse(type, value, IgnoreCase);
                    if (!Enum.IsDefined(type, parsed))
                        throw new Exception(string.Format(Resources.Cin_InputOutOfRange, type));
                    result = parsed;
                }
                else
                {
                    result = value.ToType<T>();
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