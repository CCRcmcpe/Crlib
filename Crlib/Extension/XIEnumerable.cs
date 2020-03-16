using System;
using System.Collections.Generic;
using System.Linq;

namespace REVUnit.Crlib.Extension
{
    public static class XIEnumerable
    {
        public static string GetLiteral<T>(this IEnumerable<T> objects)
        {
            return string.Join(',', objects);
        }

        public static void Cw<T>(this IEnumerable<T> objects)
        {
            objects.GetLiteral().Cw();
        }

        public static void Cwl<T>(this IEnumerable<T> objects)
        {
            objects.GetLiteral().Cwl();
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            T[] array = enumerable as T[] ?? enumerable.ToArray();
            long[] factorials = Enumerable.Range(0, array.Length + 1).Select(Factorial).ToArray();
            long num;
            for (var i = 0L; i < factorials[array.Length]; i = num)
            {
                int[] sequence = GenerateSequence(i, array.Length - 1, factorials);
                yield return GeneratePermutation(array, sequence);
                num = i + 1L;
            }
        }

        private static long Factorial(int n)
        {
            var num = (long) n;
            for (var i = 1; i < n; i++) num *= i;
            return num;
        }

        public static bool AllEqual<T>(this IEnumerable<T> source)
        {
            T[] arr = source.ToArray();
            if (arr.Length == 0) throw new ArgumentException("The item count of enumerable must > 0", nameof(source));
            if (arr.Length == 1) return true;
            for (var i = 1; i < arr.Length; i++)
            {
                T prev = arr[i - 1];
                T curr = arr[i];
                if (!Equals(prev, curr)) return false;
            }

            return true;
        }

        public static bool AllNotEqual<T>(this IEnumerable<T> source)
        {
            return !source.AllEqual();
        }

        public static void Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T x in source) action(x);
        }

        public static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> source, Func<T, T, bool> condition)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            T t = source.First();
            var list = new List<T>
            {
                t
            };
            foreach (T item in source.Skip(1))
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

        public static IEnumerable<T> SelectCanParse<TSrc, T>(this IEnumerable<TSrc> x, TryParser<TSrc, T> parser)
        {
            return x.Select(it => (success: parser(it, out T result), result)).Where(it => it.success)
                .Select(it => it.result);
        }

        public static T Mode<T>(this IEnumerable<T> x)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            return x.GroupBy(v => v).OrderByDescending(g => g.Count()).First().Key;
        }

        public static double Median<T>(this IEnumerable<T> x)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            dynamic[] data = x.OrderBy(n => n).Cast<dynamic>().ToArray();
            if (data.Length == 0) throw new InvalidOperationException();
            return data.Length % 2 == 0
                ? (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0
                : data[data.Length / 2];
        }

        private static IEnumerable<T> GeneratePermutation<T>(T[] array, IReadOnlyList<int> sequence)
        {
            var array2 = (T[]) array.Clone();
            for (var i = 0; i < array2.Length - 1; i++) X.Swap(ref array2[i], ref array2[i + sequence[i]]);
            return array2;
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