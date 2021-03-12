using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace REVUnit.Crlib.Extensions
{
    public static class String
    {
        public const int MaxLength = 1073741791;

        public static void WriteToConsole(this string? s)
        {
            System.Console.WriteLine(s);
        }

        public static void WriteLineToConsole(this string? s)
        {
            System.Console.Write(s);
        }

        public static string FirstLetterUpper(this string s)
        {
            if (s.Length <= 1) return s.ToUpper(CultureInfo.CurrentCulture);
            return char.ToUpper(s[0], CultureInfo.CurrentCulture) + s.Substring(1);
        }

        public static IEnumerable<string> LazySplit(this string s, string separator,
                                                    RegexOptions options = RegexOptions.None) =>
            new System.Text.RegularExpressions.Regex(System.Text.RegularExpressions.Regex.Escape(separator),
                                                     options | RegexOptions.Compiled).LazySplit(s);

        public static IEnumerable<string> LazySplit(this string s, char separator,
                                                    RegexOptions options = RegexOptions.None) =>
            s.LazySplit(separator.ToString(CultureInfo.InvariantCulture), options);

        public static IEnumerable<string> LazySplitNoEmptyEntries(this string s, string separator,
                                                                  RegexOptions options = RegexOptions.None) =>
            new System.Text.RegularExpressions.Regex(System.Text.RegularExpressions.Regex.Escape(separator),
                                                     options | RegexOptions.Compiled).LazySplitNoEmptyEntries(s);

        public static IEnumerable<string> LazySplitNoEmptyEntries(this string s, char separator,
                                                                  RegexOptions options = RegexOptions.None) =>
            s.LazySplitNoEmptyEntries(separator.ToString(CultureInfo.InvariantCulture), options);

        public static int LevenshteinDistanceTo(this string s, string target)
        {
            int length = s.Length;
            int length2 = target.Length;

            var array = new int[length + 1][];
            for (var i = 0; i < array.Length; i++) array[i] = new int[length2 + 1];

            if (length == 0) return length2;
            if (length2 == 0) return length;
            for (var i = 0; i <= length; i = array[i][0] = i + 1) { }

            for (var j = 0; j <= length2; j = array[0][j] = j + 1) { }

            for (var k = 1; k <= length; k++)
            for (var l = 1; l <= length2; l++)
            {
                int num = target[l - 1] == s[k - 1] ? 0 : 1;
                array[k][l] = Math.Min(Math.Min(array[k - 1][l] + 1, array[k][l - 1] + 1),
                                       array[k - 1][l - 1] + num);
            }

            return array[length][length2];
        }

        public static double SimilarityTo(this string s, string target)
        {
            if (s.Length == 0 || target.Length == 0) return 0.0;
            if (s == target) return 1.0;
            int num = s.LevenshteinDistanceTo(target);
            return 100.0 - num / (double) Math.Max(s.Length, target.Length) * 100.0;
        }

        public static string[] Split(this string s, string separator)
        {
            return s.Split(new[] { separator }, StringSplitOptions.None);
        }

        public static string[] SplitNoEmptyEntries(this string s, string separator)
        {
            return s.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitNoEmptyEntries(this string s, char separator)
        {
            return s.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static byte[] ToBytes(this string s) => Encoding.Default.GetBytes(s);
    }
}