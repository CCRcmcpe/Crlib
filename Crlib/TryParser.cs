using System;
using System.Diagnostics.CodeAnalysis;

namespace REVUnit.Crlib
{
    public class TryParser<TSrc, T>
    {
        public delegate bool Agent(TSrc value, [MaybeNullWhen(false)] out T result);

        private readonly Func<TSrc, T> _parser;

        public TryParser(Func<TSrc, T> parser) => _parser = parser;

        public bool TryParse(TSrc value, [MaybeNullWhen(false)] out T result)
        {
            try
            {
                result = _parser(value);
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