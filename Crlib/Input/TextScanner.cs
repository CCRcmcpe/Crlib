using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace REVUnit.Crlib.Input
{
    public abstract class TextScanner
    {
        private List<char> _charsRead;
        private string _tokenDelimiter;

#pragma warning disable CS8618 // 不可为 null 的字段未初始化。请考虑声明为可以为 null。
        protected TextScanner(TextReader source, string? tokenDelimiter = null)
#pragma warning restore CS8618 // 不可为 null 的字段未初始化。请考虑声明为可以为 null。
        {
            Source = source;
            TokenDelimiter = tokenDelimiter ?? " ";
        }

        public TextReader Source { get; protected set; }
        public bool IgnoreCase { get; set; } = true;
        public bool AutoTrim { get; set; }

        public string TokenDelimiter
        {
            get => _tokenDelimiter;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentOutOfRangeException(nameof(value));
                _charsRead = new List<char>(value.Length);
                _tokenDelimiter = value;
            }
        }

        public bool IsOnEof { get; protected set; }

        public abstract T Get<T>() where T : IConvertible;

        protected string? NextToken()
        {
            if (IsOnEof) return null;

            var stringBuilder = new StringBuilder();

            while (true)
            {
                int read = Source.Read();

                if (read == -1)
                {
                    IsOnEof = true;
                    break;
                }

                if (_charsRead.Count + 1 > TokenDelimiter.Length) _charsRead.RemoveAt(0);

                var ch = (char) read;
                _charsRead.Add(ch);
                stringBuilder.Append(ch);

                if (new string(_charsRead.ToArray()) == TokenDelimiter)
                {
                    _charsRead.Clear();
                    stringBuilder.Remove(stringBuilder.Length - TokenDelimiter.Length, TokenDelimiter.Length);
                    break;
                }
            }

            var notTrimmed = stringBuilder.ToString();
            return AutoTrim ? notTrimmed.Trim() : notTrimmed;
        }
    }
}