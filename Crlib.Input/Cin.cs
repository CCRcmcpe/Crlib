using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using REVUnit.Crlib.Extension;
using REVUnit.Crlib.Input.Properties;

namespace REVUnit.Crlib.Input
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string? faultedToken, string? message = null)
        {
            Message = message ?? string.Format(Resources.InvalidInputException_Message, FaultedToken);
            FaultedToken = faultedToken;
        }

        public string? FaultedToken { get; set; }
        public override string Message { get; }

        public override IDictionary Data => new ListDictionary { { "FaultedToken", FaultedToken } };
    }

    public class Cin : Scanner
    {
        public delegate Exception? Parser<T>(string value, out T? result);

        public Cin() : base(Console.In, Environment.NewLine) { }

        public bool WriteEnumDescription { get; set; } = true;
        public bool ThrowOnUndefinedEnum { get; set; } = true;
        public Exception? LastException { get; private set; }

        public T? Get<T>(string? hint = null) where T : IConvertible => Get(hint, (Parser<T>) TryParse);

        public T? Get<T>(string? hint, Parser<T> parser)
        {
            if (Eof) return default;

            bool interactive = Environment.UserInteractive;

            if (string.IsNullOrWhiteSpace(hint))
                hint = string.Empty;
            else if (!hint.EndsWith(": "))
                hint += ": ";


            Type type = typeof(T);
            if (type.IsEnum && interactive && WriteEnumDescription)
            {
                Console.WriteLine(GetEnumDescription(type));
            }

            while (true)
            {
                if (interactive) Console.Write(hint);
                string? token = NextToken();
                if (token == null) return default;

                LastException = parser(token, out T? result);
                return LastException == null ? result : default;
            }
        }

        private static string GetEnumDescription(Type enumType)
        {
            return enumType.GetEnumValues().Cast<IConvertible>()
                           .Select(v => v.ToType(enumType.GetEnumUnderlyingType())!)
                           .Select(v => $"[{v}]={enumType.GetEnumName(v)}").GetLiteral();
        }

        private Exception? TryParse<T>(string value, out T? result) where T : IConvertible
        {
            result = default;
            Type type = typeof(T);

            try
            {
                if (!type.IsEnum)
                {
                    object? o = result.ToType(type);
                    result = o != null ? (T?) o : default;
                }

                result = (T) Enum.Parse(type, value, IgnoreCase);
            }
            catch (Exception e)
            {
                return e;
            }

            if (ThrowOnUndefinedEnum && !Enum.IsDefined(type, result))
            {
                throw new InvalidInputException(string.Format(Resources.Cin_InputOutOfRange, type));
            }

            return null;
        }
    }
}