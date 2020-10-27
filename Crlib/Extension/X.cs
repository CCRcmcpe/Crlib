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
        ///     此函数以 <paramref name="probability" /> 的概率返回 <c>true</c>，使用 <see cref="Random" /> 类。
        /// </summary>
        public static bool HappensProbability(float probability)
        {
            var random = new Random();
            return HappensProbability(probability, random.Next);
        }

        /// <summary>
        ///     此函数以 <paramref name="probability" /> 的概率返回 <c>true</c>，使用闭包随机数生成器 <paramref name="randomGenerator" />。
        ///     闭包的第一个参数应为随机数生成闭区间的下界，第二个参数应为上界，返回生成的随机数。
        /// </summary>
        public static bool HappensProbability(float probability, Func<int, int, int> randomGenerator)
        {
            float f = 1 / probability;
            var i = 1;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (f - Math.Truncate(f) == 0) return randomGenerator(0, (int) f) == 0;
            while (Math.Abs(f * i - Math.Truncate(f * i)) > float.Epsilon) i++;
            return randomGenerator(0, i) < i;
        }

        /// <summary>
        ///     此函数以 <paramref name="probability" /> 的概率返回 <c>true</c>，使用 <see cref="RandomNumberGenerator" /> 类。
        /// </summary>
        public static bool HappensProbabilityS(float probability) =>
            HappensProbability(probability, RandomNumberGenerator.GetInt32);

        /// <summary>
        ///     使用 <see cref="Stopwatch" /> 计量执行一次 <paramref name="action" /> 的时间。
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
        ///     使用 <see cref="Stopwatch" /> 计量执行 <paramref name="avgTime" /> 次 <paramref name="action" /> 的平均时间。
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
        ///     当 <paramref name="condiction" /> 为 <c>true</c> 时返回，否则循环。
        /// </summary>
        public static void WaitUntil(Func<bool> condiction)
        {
            if (condiction == null) throw new ArgumentNullException(nameof(condiction));
            while (condiction()) return;
        }

        /// <summary>
        ///     当 <paramref name="condiction" /> 为 <c>true</c> 时返回，否则循环，每次循环会有 <paramref name="cycleInterval" /> 的间隔。
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
        ///     求值 <paramref name="function" />，当 <paramref name="condiction" /> 为 <c>true</c> 时返回值，否则循环。
        /// </summary>
        [return: MaybeNull]
        public static T While<T>(Func<T> function, Predicate<T> condiction)
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
        ///     求值 <paramref name="function" />，当 <paramref name="condiction" />为 <c>true</c> 时返回值，否则循环。
        /// </summary>
        [return: MaybeNull]
        public static T While<T>(Func<T> function, Predicate<T> condiction,
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
        ///     求值 <paramref name="function" />，当 <paramref name="condiction" /> 为 <c>true</c> 时返回值，否则循环，当循环
        ///     <paramref name="maxRetry" />
        ///     次后将抛出异常。
        /// </summary>
        [return: MaybeNull]
        public static T While<T>(Func<T> function, Predicate<T> condiction, int maxRetry)
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
        ///     求值 <paramref name="function" />，当 <paramref name="condiction" /> 为 <c>true</c> 时返回值，否则循环，每次循环会有
        ///     <paramref name="cycleInterval" /> 的间隔，当循环 <paramref name="maxRetry" /> 次后将抛出异常。
        /// </summary>
        [return: MaybeNull]
        public static T While<T>(Func<T> function, Predicate<T> condiction,
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
    }
}