using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using REVUnit.Crlib.Properties;

namespace REVUnit.Crlib.Extension
{
    public static class XIList
    {
        public static bool AllEqual<T>(this IList<T> source) where T : IEquatable<T>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.AllEqual((a, b) => !a.Equals(b));
        }

        public static bool AllEqual<T>(this IList<T> source, Func<T, T, bool> comparer)
        {
            int length = source.Count;
            if (length == 0) throw new ArgumentException(Resources.XIList_Exception_ListEmpty, nameof(source));
            if (length == 1) return true;
            T prev = source[0];
            for (var i = 1; i < length; i++)
            {
                T curr = source[i];
                if (!comparer(curr, prev)) return false;

                prev = curr;
            }

            return true;
        }

        public static void Fill<T>(this IList<T> list, T value)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            for (var i = 0; i < list.Count; i++) list[i] = value;
        }

        public static void Fill<T>(this IList<T> list, Func<T> valueGenerator)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (valueGenerator == null) throw new ArgumentNullException(nameof(valueGenerator));
            for (var i = 0; i < list.Count; i++) list[i] = valueGenerator();
        }

        public static T? ParallelFind<T>(this IList<T> list, Predicate<T> predicate)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            T? result = default;
            Parallel.For(0, list.Count - 1, (i, state) =>
            {
                T t = list[i];
                if (predicate(t))
                {
                    result = t;
                    state.Stop();
                }
            });
            return result;
        }

        public static T[] ParallelFindAll<T>(this IList<T> list, Predicate<T> predicate)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            using var matches = new BlockingCollection<T>();
            Parallel.For(0, list.Count - 1, (i, _) =>
            {
                T t = list[i];
                // ReSharper disable once AccessToDisposedClosure
                if (predicate(t)) matches.Add(t);
            });
            T[] result = matches.ToArray();

            return result;
        }

        public static int ParallelFindIndex<T>(this IList<T> list, Predicate<T> predicate)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (list == null) throw new ArgumentNullException(nameof(list));
            int index = -1;
            Parallel.For(0, list.Count - 1, (i, state) =>
            {
                if (!predicate(list[i])) return;
                index = i;
                state.Stop();
            });
            return index;
        }
    }
}