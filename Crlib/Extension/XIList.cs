using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace REVUnit.Crlib.Extension
{
    public static class XIList
    {
        public static void Fill<T>(this IList<T> list, T value)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            for (var i = 0; i < list.Count; i++) list[i] = value;
        }

        public static void Fill<T>(this IList<T> list, Func<T> valueGenerator)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            for (var i = 0; i < list.Count; i++) list[i] = valueGenerator();
        }

        public static T RandomGet<T>(this IList<T> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            return list[new Random().Next(list.Count)];
        }

        public static void RandomFill<T>(this IList<T> list, int time, T value)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            var random = new Random();
            for (var i = 0; i < time; i++) list[random.Next(0, list.Count - 1)] = value;
        }

        public static void RandomFill<T>(this IList<T> list, int time, Func<T> valueGenerator)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            var random = new Random();
            for (var i = 0; i < time; i++) list[random.Next(0, list.Count - 1)] = valueGenerator();
        }

        public static T ParallelFind<T>(this IList<T> list, Predicate<T> predicate)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            T result = default;
            Parallel.For(0, list.Count - 1, delegate(int i, ParallelLoopState state)
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
            T[] result;
            using (var matches = new BlockingCollection<T>())
            {
                Parallel.For(0, list.Count - 1, delegate(int i, ParallelLoopState state)
                {
                    T t = list[i];
                    if (predicate(t)) matches.Add(t);
                });
                result = matches.ToArray();
            }

            return result;
        }

        public static int ParallelFindIndex<T>(this IList<T> list, Predicate<T> predicate)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            int index = -1;
            Parallel.For(0, list.Count - 1, delegate(int i, ParallelLoopState state)
            {
                T obj = list[i];
                if (!predicate(obj)) return;
                index = -1;
                state.Stop();
            });
            return index;
        }
    }
}