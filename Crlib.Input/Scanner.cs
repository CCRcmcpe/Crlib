using System;
using System.IO;

namespace REVUnit.Crlib.Input
{
    public class Scanner : TextScanner
    {
        public Scanner(TextReader source, string delimiterRegex) : base(source, delimiterRegex) { }

        public bool IgnoreCase { get; set; }

        public T? Next<T>(IFormatProvider? formatProvider) where T : IConvertible
        {
            string? token = NextToken();
            if (token == null)
            {
                return default;
            }

            try
            {
                Type targetType = typeof(T);
                return targetType.IsEnum
                    ? (T) Enum.Parse(targetType, token, IgnoreCase)
                    : (T) ((IConvertible) token).ToType(targetType, formatProvider);
            }
            catch
            {
                throw new InvalidTokenException(token);
            }
        }
    }
}