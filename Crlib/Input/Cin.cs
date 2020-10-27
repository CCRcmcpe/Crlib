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
        public InvalidInputException(string? errorToken) => ErrorToken = errorToken;

        public string? ErrorToken { get; set; }
        public override string Message => string.Format(Resources.InvalidInputException_Message, ErrorToken);
        public override IDictionary Data => new ListDictionary { { "ErrorToken", ErrorToken } };
    }

    public class Cin : TextScanner
    {
        public Cin(string? lineSeparator = null) : base(Console.In, lineSeparator ?? Environment.NewLine) { }

        public bool WriteEnumDescription { get; set; }
        public bool ThrowOnInvalidInput { get; set; }
        public bool ThrowOnUndefinedEnum { get; set; }

        [return: MaybeNull]
        public override T Get<T>() => Get(null, Parse<T>);

        [return: MaybeNull]
        public T Get<T>(string? hint) where T : IConvertible => Get(hint, Parse<T>);

        [return: MaybeNull]
        public T Get<T>(string? hint, Func<string, T> parser)
        {
            if (IsOnEof) return default;
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
                if (token == null) return default;
                try
                {
                    return parser(token);
                }
                catch (Exception e)
                {
                    if (ThrowOnInvalidInput || !interactive) throw new InvalidInputException(token);
                    Console.WriteLine(Resources.Cin_InvalidInput, token, e.Message);
                    hint = Resources.Cin_EnterAgainHint;
                }
            }
        }

        private static string GetEnumDescription(Type enumType)
        {
            return enumType.GetEnumValues().Cast<IConvertible>()
                           .Select(it => it!.ToType(enumType.GetEnumUnderlyingType()))
                           .Select(it => $"[{it}]={enumType.GetEnumName(it)}").GetLiteral();
        }

        private T Parse<T>(string value) where T : IConvertible
        {
            T ret;
            Type type = typeof(T);
            if (type.IsEnum)
            {
                var parsed = (T) Enum.Parse(type, value, IgnoreCase);
                if (ThrowOnUndefinedEnum && !Enum.IsDefined(type, parsed))
                    throw new Exception(string.Format(Resources.Cin_InputOutOfRange, type));
                ret = parsed;
            }
            else
                ret = value.ToType<T>();

            return ret;
        }
    }
}