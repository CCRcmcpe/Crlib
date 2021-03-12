using System;
using System.Diagnostics;

namespace REVUnit.Crlib
{
    internal static class ExecuteTime
    {
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
        ///     使用 <see cref="Stopwatch" /> 计量执行 <paramref name="averageTimes" /> 次 <paramref name="action" /> 的平均时间。
        /// </summary>
        public static TimeSpan MeasureAverage(Action action, int averageTimes)
        {
            if (averageTimes < 1) throw new ArgumentOutOfRangeException(nameof(averageTimes));
            var stopwatch = new Stopwatch();
            TimeSpan time = TimeSpan.Zero;
            for (var i = 0; i < averageTimes; i++)
            {
                stopwatch.Start();
                action();
                stopwatch.Stop();
                time += stopwatch.Elapsed;
                stopwatch.Reset();
            }

            return time / averageTimes;
        }
    }
}