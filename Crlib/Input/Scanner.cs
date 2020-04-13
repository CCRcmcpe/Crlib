using System;
using System.Globalization;
using System.IO;
using System.Text;
using REVUnit.Crlib.Extension;

namespace REVUnit.Crlib.Input
{
    public sealed class Scanner
    {
        private readonly TextReader _src;

        public Scanner(bool ignoreCase = false)
        {
            IgnoreCase = ignoreCase;
            _src = Console.In;
        }

        public Scanner(TextReader source)
        {
            _src = source;
        }

        public bool IgnoreCase { get; set; }

        public string WaitingToken { get; private set; }

        public bool NextToken()
        {
            var stringBuilder = new StringBuilder();

            while (true)
            {
                int read = _src.Read();
                if (read == -1) return false;

                var ch = (char) read;
                if (char.IsWhiteSpace(ch))
                {
                    WaitingToken = stringBuilder.ToString();
                    return true;
                }

                stringBuilder.Append(ch);
            }
        }

        public bool HasNext<T>() where T : IConvertible
        {
            return !typeof(T).IsEnum
                ? WaitingToken.TryToType<T>(out _)
                : Enum.TryParse(typeof(T), WaitingToken, IgnoreCase, out _);
        }

        private string TakeToken()
        {
            if (WaitingToken == null) NextToken();
            string waitingToken = WaitingToken;
            WaitingToken = null;
            return waitingToken;
        }

        public T Next<T>() where T : IConvertible
        {
            string text = TakeToken();
            try
            {
                return typeof(T).IsEnum
                    ? (T) Enum.Parse(typeof(T), text, IgnoreCase)
                    : (T) ((IConvertible) text).ToType(typeof(T), CultureInfo.CurrentCulture);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Invalid value \"{text}\".");
                throw;
            }
        }
    }
}