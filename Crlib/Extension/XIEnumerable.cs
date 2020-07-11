using System;
using System.Collections.Generic;
using System.Linq;

namespace REVUnit.Crlib.Extension
{
    public static class XIEnumerable
    {
        public static bool AllEqual<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.AllEqual((a, b) => a != null && !a.Equals(b));
        }

        public static bool AllEqual<T>(this IEnumerable<T> source, Func<T, T, bool> comparer)
        {
            T[] array = source.ToArray();
            int length = array.Length;
            if (length == 0) throw new ArgumentException("The enumerable must not be empty", nameof(source));
            if (length == 1) return true;
            T prev = array[0];
            for (var i = 1; i < length; i++)
            {
                T curr = array[i];
                if (!comparer(curr, prev)) return false;

                prev = curr;
            }

            return true;
        }

        public static void Cl<T>(this IEnumerable<T> objects)
        {
            objects.GetLiteral().Cl();
        }

        public static void Cw<T>(this IEnumerable<T> objects)
        {
            objects.GetLiteral().Cw();
        }

        public static void Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T x in source) action(x);
        }

        public static string GetLiteral<T>(this IEnumerable<T> objects)
        {
            return $"[{string.Join(", ", objects)}]";
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

        public static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> source, Func<T, T, bool> condition)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            IEnumerable<T> enumerable = source as T[] ?? source.ToArray();
            T t = enumerable.First();
            var list = new List<T>
            {
                t
            };
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

        public static double Median<T>(this IEnumerable<T> x)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            dynamic[] data = x.OrderBy(n => n).Cast<dynamic>().ToArray();
            if (data.Length == 0) throw new InvalidOperationException();
            return data.Length % 2 == 0
                ? (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0
                : data[data.Length / 2];
        }

        public static T Mode<T>(this IEnumerable<T> x)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            return x.GroupBy(v => v).OrderByDescending(g => g.Count()).First().Key;
        }

        public static IEnumerable<T> SelectCanParse<TSrc, T>(this IEnumerable<TSrc> x, TryParser<TSrc, T> parser)
        {
            return x.Select(it => (success: parser(it, out T result), result)).Where(it => it.success)
                .Select(it => it.result);
        }

        private static long Factorial(int n)
        {
            var num = (long) n;
            for (var i = 1; i < n; i++) num *= i;
            return num;
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