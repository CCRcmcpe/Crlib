using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace REVUnit.Crlib.Extension
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
            if (s.Length <= 1) return s.ToUpper(CultureInfo.InvariantCulture);
            return char.ToUpper(s[0], CultureInfo.InvariantCulture) + s.Substring(1);
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

        public static string Numeral(this string s, int system)
        {
            var stringBuilder = new StringBuilder();
            foreach (char value in s) stringBuilder.Append(Convert.ToString(value, system).PadLeft(8, '0'));
            return stringBuilder.ToString();
        }

        public static double[] ScanDoubles(this string s)
        {
            var d = 0.0;
            return System.Text.RegularExpressions.Regex.Matches(s, @"-?\d+(\.\d+)?")
                         .Where(m => double.TryParse(m.Value, out d)).Select(_ => d)
                         .ToArray();
        }

        public static float[] ScanFloats(this string s)
        {
            var d = 0f;
            return System.Text.RegularExpressions.Regex.Matches(s, @"-?\d+(\.\d+)?")
                         .Where(m => float.TryParse(m.Value, out d)).Select(_ => d)
                         .ToArray();
        }

        public static int[] ScanInts(this string s)
        {
            var i = 0;
            return System.Text.RegularExpressions.Regex.Matches(s, @"-?\d+").Where(m => int.TryParse(m.Value, out i))
                         .Select(_ => i).ToArray();
        }

        public static uint[] ScanUints(this string s)
        {
            var i = 0u;
            return System.Text.RegularExpressions.Regex.Matches(s, @"\d+").Where(m => uint.TryParse(m.Value, out i))
                         .Select(_ => i).ToArray();
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

        public static StringBuilder StringBuilder(this string? s) => new(s);

        public static byte[] ToBytes(this string s) => Encoding.Default.GetBytes(s);
    }
}