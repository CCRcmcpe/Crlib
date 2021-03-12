using System;
using System.Diagnostics;

namespace REVUnit.Crlib
{
    internal static class ExecuteTime
    {
        /// <summary>
        ///     ʹ�� <see cref="Stopwatch" /> ����ִ��һ�� <paramref name="action" /> ��ʱ�䡣
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
        ///     ʹ�� <see cref="Stopwatch" /> ����ִ�� <paramref name="averageTimes" /> �� <paramref name="action" /> ��ƽ��ʱ�䡣
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