using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using REVUnit.Crlib.Input.Properties;

namespace REVUnit.Crlib.Input
{
    public class TextScanner
    {
        public TextScanner(TextReader source, Regex delimiterRegex)
        {
            Source = source;
            DelimiterRegex = delimiterRegex;
        }

        public TextScanner(TextReader source, string delimiterRegex)
        {
            if (string.IsNullOrEmpty(delimiterRegex))
            {
                throw new ArgumentOutOfRangeException(nameof(delimiterRegex), delimiterRegex,
                                                      Resources.TextScanner_Exception_DelimiterRegexNullOrEmpty);
            }

            Source = source;
            DelimiterRegex = new Regex(delimiterRegex, RegexOptions.Compiled);
        }

        public TextReader Source { get; }

        public Regex DelimiterRegex { get; set; }

        public bool Eof => Source.Peek() == -1;

        public string? NextToken()
        {
            if (Eof) return null;

            var tokenBuilder = new StringBuilder();

            while (!Eof)
            {
                var c = (char) Source.Read();

                tokenBuilder.Append(c);
                var input = tokenBuilder.ToString();

                Match match = DelimiterRegex.Match(input);
                if (!match.Success) continue;

                tokenBuilder.Remove(match.Index, match.Length);
                break;
            }

            return tokenBuilder.ToString();
        }
    }
}