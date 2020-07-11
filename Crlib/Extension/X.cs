using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Threading;

namespace REVUnit.Crlib.Extension
{
    public static class X
    {
        /// <summary>
        ///     以<paramref name="probability" />的概率返回<c>true</c>，使用<see cref="Random" />。
        /// </summary>
        public static bool HappensProbability(float probability)
        {
            var random = new Random();
            float f = 1 / probability;
            var i = 2;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (f - Math.Truncate(f) != 0)
            {
                while (Math.Abs(f * i - Math.Truncate(f * i)) > float.Epsilon) i++;

                return random.Next(0, (int) f * i) < i;
            }

            return random.Next(0, (int) f) == 0;
        }

        /// <summary>
        ///     以<paramref name="probability" />的概率返回<c>true</c>，使用<see cref="RandomNumberGenerator" />。
        /// </summary>
        public static bool HappensProbabilityS(float probability)
        {
            float f = 1 / probability;
            var i = 1;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (f - Math.Truncate(f) != 0)
            {
                while (Math.Abs(f * i - Math.Truncate(f * i)) > float.Epsilon) i++;

                return RandomNumberGenerator.GetInt32(0, i) < i;
            }

            return RandomNumberGenerator.GetInt32(0, (int) f) == 0;
        }

        /// <summary>
        ///     使用<see cref="Stopwatch" />计量执行一次<paramref name="action" />的时间。
        /// </summary>
        public static TimeSpan Measure(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            var sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            return sw.Elapsed;
        }

        /// <summary>
        ///     使用<see cref="Stopwatch" />计量执行一次<paramref name="action" />的时间。
        /// </summary>
        public static TimeSpan MeasureAvg(Action action, int avgTime)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (avgTime < 1) throw new ArgumentOutOfRangeException(nameof(avgTime));
            var stopwatch = new Stopwatch();
            TimeSpan time = TimeSpan.Zero;
            for (var i = 0; i < avgTime; i++)
            {
                stopwatch.Start();
                action();
                stopwatch.Stop();
                time += stopwatch.Elapsed;
                stopwatch.Reset();
            }

            return time / avgTime;
        }

        /// <summary>
        ///     交换两个同类型的引用。
        /// </summary>
        public static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        /// <summary>
        ///     当<paramref name="condiction" />为true时返回，否则循环。
        /// </summary>
        public static void WaitUntil(Func<bool> condiction)
        {
            if (condiction == null) throw new ArgumentNullException(nameof(condiction));
            while (condiction()) return;
        }

        /// <summary>
        ///     当<paramref name="condiction" />为true时返回，否则循环，每次循环会有<paramref name="cycleInterval" />的间隔。
        /// </summary>
        public static void WaitUntil(Func<bool> condiction, TimeSpan cycleInterval)
        {
            var flag = false;
            while (true)
            {
                TimeSpan time = Measure(() => flag = condiction());
                if (flag) return;
                if (time < cycleInterval) Thread.Sleep(cycleInterval - time);
            }
        }

        /// <summary>
        ///     求值<paramref name="function" />，当<paramref name="condiction" />为true时返回值，否则循环。
        /// </summary>
        [return: MaybeNull]
        public static T While<T>(Func<T> function, Func<T, bool> condiction)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            if (condiction == null) throw new ArgumentNullException(nameof(condiction));
            while (true)
            {
                T result = function();
                if (condiction(result)) return result;
            }
        }

        /// <summary>
        ///     求值<paramref name="function" />，当<paramref name="condiction" />为true时返回值，否则循环。
        /// </summary>
        [return: MaybeNull]
        public static T While<T>(Func<T> function, Func<T, bool> condiction,
            TimeSpan cycleInterval)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            if (condiction == null) throw new ArgumentNullException(nameof(condiction));
            T result = default;
            while (true)
            {
                TimeSpan time = Measure(() => result = function());
                if (condiction(result!)) return result;
                if (time < cycleInterval) Thread.Sleep(cycleInterval - time);
            }
        }

        /// <summary>
        ///     求值<paramref name="function" />，当<paramref name="condiction" />为true时返回值，否则循环，当循环<paramref name="maxRetry" />
        ///     次后将引发错误。
        /// </summary>
        [return: MaybeNull]
        public static T While<T>(Func<T> function, Func<T, bool> condiction, int maxRetry)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            if (condiction == null) throw new ArgumentNullException(nameof(condiction));
            var retried = 0;
            while (true)
            {
                if (++retried == maxRetry) throw new Exception("Max retry reached");
                T result = function();
                if (condiction(result)) return result;
            }
        }

        /// <summary>
        ///     求值<paramref name="function" />，当<paramref name="condiction" />为true时返回值，否则循环，每次循环会有
        ///     <paramref name="cycleInterval" />的间隔，当循环<paramref name="maxRetry" />次后将引发错误。
        /// </summary>
        [return: MaybeNull]
        public static T While<T>(Func<T> function, Func<T, bool> condiction,
            TimeSpan cycleInterval, int maxRetry)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            if (condiction == null) throw new ArgumentNullException(nameof(condiction));
            var retried = 0;
            T result = default;
            while (true)
            {
                if (++retried == maxRetry) throw new Exception("Max retry reached");
                TimeSpan time = Measure(() => result = function());
                if (time < cycleInterval) Thread.Sleep(cycleInterval - time);
                if (condiction(result!)) return result;
            }
        }

        /// <summary>
        ///     求值<paramref name="function" />，当<paramref name="tryParser" />为true时返回它out的值，否则循环，每次循环会有
        ///     <paramref name="cycleInterval" />的间隔。
        /// </summary>
        [return: MaybeNull]
        public static T While<TSrc, T>(Func<TSrc> function, TryParser<TSrc, T> tryParser,
            TimeSpan cycleInterval)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            if (tryParser == null) throw new ArgumentNullException(nameof(tryParser));
            TSrc value = default;
            while (true)
            {
                TimeSpan time = Measure(() => value = function());
                if (time < cycleInterval) Thread.Sleep(cycleInterval - time);
                if (tryParser(value!, out T result)) return result;
            }
        }

        /// <summary>
        ///     求值<paramref name="function" />，当<paramref name="tryParser" />为true时返回它out的值，否则循环，每次循环会有
        ///     <paramref name="cycleInterval" />的间隔，当循环<paramref name="maxRetry" />次后将引发错误。
        /// </summary>
        [return: MaybeNull]
        public static T While<TSrc, T>(Func<TSrc> function, TryParser<TSrc, T> tryParser,
            TimeSpan cycleInterval, int maxRetry)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            if (tryParser == null) throw new ArgumentNullException(nameof(tryParser));
            var retried = 0;
            TSrc value = default;
            while (true)
            {
                if (++retried == maxRetry) throw new Exception("Max retry reached");
                TimeSpan time = Measure(() => value = function());
                if (tryParser(value!, out T result)) return result;
                if (time < cycleInterval) Thread.Sleep(cycleInterval - time);
            }
        }

        /// <summary>
        ///     求值<paramref name="function" />，当<paramref name="tryParser" />返回值的第一项为true时返回它第二项的值，否则循环，每次循环会有
        ///     <paramref name="cycleInterval" />的间隔。
        /// </summary>
        [return: MaybeNull]
        public static T While<TSrc, T>(Func<TSrc> function, Func<TSrc, (bool, T)> tryParser,
            TimeSpan cycleInterval)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));
            if (tryParser == null) throw new ArgumentNullException(nameof(tryParser));
            TSrc value = default;
            while (true)
            {
                TimeSpan time = Measure(() => value = function());
                (bool success, T result) = tryParser(value!);
                if (success) return result;
                if (time < cycleInterval)
                    Thread.Sleep(cycleInterval - time); //如果计算的时间已经超过周期间隔时间，不同步到下一个周期，直接继续
            }
        }
    }
}