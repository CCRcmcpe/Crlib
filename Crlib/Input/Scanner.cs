using System;
using System.Globalization;
using System.IO;
using REVUnit.Crlib.Properties;

namespace REVUnit.Crlib.Input
{
    public class Scanner : TextScanner
    {
        public Scanner(TextReader source) : base(source) { }

        public override T Get<T>()
        {
            string? token = NextToken();
            if (token == null)
            {
                IsOnEof = true;
                return default!;
            }

            try
            {
                return typeof(T).IsEnum
                    ? (T) Enum.Parse(typeof(T), token, IgnoreCase)
                    : (T) ((IConvertible) token).ToType(typeof(T), CultureInfo.CurrentCulture);
            }
            catch
            {
                Console.WriteLine(Resources.Scanner_InvalidValue, token);
                throw;
            }
        }
    }
}