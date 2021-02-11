using System;
using System.Diagnostics;
using System.Threading;

namespace REVUnit.Crlib.Extension
{
    public static class X
    {
        /// <summary>
        ///     此方法有 <paramref name="probability" /> 的概率返回 <c>true</c>，使用 <see cref="System.Random" /> 类。
        /// </summary>
        public static bool HappensProbability(float probability)
        {
            var random = new System.Random();
            return HappensProbability(probability, random.Next);
        }

        /// <summary>
        ///     此方法有 <paramref name="probability" /> 的概率返回 <c>true</c>，使用随机数生成器 <paramref name="randomGenerator" />。
        ///     <paramref name="randomGenerator" /> 的第一个参数为随机数生成闭区间的下界，第二个参数应为上界，返回生成的随机数。
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
        ///     使用 <see cref="Stopwatch" /> 计量执行一次 <paramref name="action" /> 的时间。
        /// </summary>
        public static TimeSpan Measure(Action action)
        {
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
    }
}