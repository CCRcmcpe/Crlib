using System;
using System.Diagnostics;

namespace REVUnit.Crlib.Debugging
{
    public static class SpeedTest
    {
        public static void NormalTest(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.Elapsed.TotalMilliseconds}ms");
        }

        public static void AverageTest(Action action, int avgTime = 5)
        {
            var stopwatch = new Stopwatch();
            var num = 0.0;
            for (var i = 0; i < avgTime; i++)
            {
                stopwatch.Start();
                action();
                stopwatch.Stop();
                num += stopwatch.Elapsed.TotalMilliseconds;
                stopwatch.Reset();
            }

            Console.WriteLine($"{num / avgTime}ms");
        }

        public static void WeightedAverageTest(Action action, int avgTime = 20)
        {
            var stopwatch = new Stopwatch();
            var averager = new Averager(avgTime);
            for (var i = 0; i < avgTime; i++)
            {
                stopwatch.Start();
                action();
                stopwatch.Stop();
                averager.Push((decimal) stopwatch.Elapsed.TotalMilliseconds);
                stopwatch.Reset();
            }

            Console.WriteLine($"{averager.CurrentWma}ms");
        }
    }
}