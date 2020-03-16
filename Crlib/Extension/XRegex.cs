using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace REVUnit.Crlib.Extension
{
    public static class XRegex
    {
        public static IEnumerable<Match> LazyMatch(this Regex regex, string input)
        {
            if (regex == null) throw new ArgumentNullException(nameof(regex));
            Match match = regex.Match(input);
            while (match.Success)
            {
                yield return match;
                match = match.NextMatch();
            }
        }

        public static IEnumerable<string> LazySplit(this Regex regex, string input, int startAt = 0)
        {
            if (regex == null) throw new ArgumentNullException(nameof(regex));
            int num = startAt;
            Match match = regex.Match(input);
            while (match.Success)
            {
                yield return input.Substring(num, match.Index - num);
                num = match.Index + match.Length;
                match = match.NextMatch();
            }

            yield return input.Substring(num);
        }

        public static IEnumerable<string> LazySplitNoEmptyEntries(this Regex regex, string input, int startAt = 0)
        {
            if (regex == null) throw new ArgumentNullException(nameof(regex));
            int num = startAt;
            Match match = regex.Match(input);
            while (match.Success)
            {
                int num2 = num;
                int length = match.Index - num2;
                string text = input.Substring(num2, length);
                if (!string.IsNullOrEmpty(text)) yield return text;
                num = match.Index + match.Length;
                match = match.NextMatch();
            }

            yield return input.Substring(num);
        }
    }
}