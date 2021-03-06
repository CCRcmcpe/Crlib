using System;
using System.Collections.Generic;
using System.Linq;

namespace REVUnit.Crlib.Extensions
{
    public static class Enumerable
    {
        public static void WriteToConsole<T>(this IEnumerable<T> objects)
        {
            objects.GetLiteral().WriteToConsole();
        }

        public static void WriteLineToConsole<T>(this IEnumerable<T> objects)
        {
            objects.GetLiteral().WriteLineToConsole();
        }

        public static void Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T x in source) action(x);
        }

        public static string GetLiteral<T>(this IEnumerable<T> objects) => $"[{string.Join(", ", objects)}]";

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IList<T> list)
        {
            long[] factorials = System.Linq.Enumerable.Range(0, list.Count + 1).Select(Factorial).ToArray();
            long num;
            for (var i = 0L; i < factorials[list.Count]; i = num)
            {
                int[] sequence = GenerateSequence(i, list.Count - 1, factorials);
                yield return GeneratePermutation(list, sequence);
                num = i + 1L;
            }
        }

        public static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> source, Func<T, T, bool> condition)
        {
            IEnumerable<T> enumerable = source as T[] ?? source.ToArray();
            T t = enumerable.First();
            var list = new List<T> { t };
            foreach (T item in enumerable.Skip(1))
            {
                if (!condition(t, item))
                {
                    yield return list;
                    list = new List<T>();
                }

                list.Add(item);
                t = item;
            }
        }

        public static double Median(this IEnumerable<double> x)
        {
            double[] data = x.OrderBy(n => n).ToArray();
            if (data.Length == 0) throw new InvalidOperationException();
            return data.Length % 2 == 0
                ? (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0
                : data[data.Length / 2];
        }

        public static T Mode<T>(this IEnumerable<T> x)
        {
            return x.GroupBy(v => v).OrderByDescending(g => g.Count()).First().Key;
        }

        private static long Factorial(int n)
        {
            var num = (long) n;
            for (var i = 1; i < n; i++) num *= i;
            return num;
        }

        private static IEnumerable<T> GeneratePermutation<T>(IList<T> list, IReadOnlyList<int> sequence)
        {
            T[] array2 = list.ToArray();
            for (var i = 0; i < array2.Length - 1; i++) Swap(ref array2[i], ref array2[i + sequence[i]]);
            return array2;
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        private static int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
        {
            var array = new int[size];
            for (var i = 0; i < array.Length; i++)
            {
                long num = factorials[array.Length - i];
                array[i] = (int) (number / num);
                number = (int) (number % num);
            }

            return array;
        }
    }
}