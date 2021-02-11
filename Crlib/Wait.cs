using System;
using System.Threading;

namespace REVUnit.Crlib
{
    internal static class Wait
    {
        /// <summary>
        ///     当 <paramref name="condiction" /> 为 <c>true</c> 时返回，否则循环。
        /// </summary>
        public static void Until(Func<bool> condiction)
        {
            while (condiction()) return;
        }

        /// <summary>
        ///     当 <paramref name="condiction" /> 为 <c>true</c> 时返回，否则循环，每次循环会有 <paramref name="cycleInterval" /> 的间隔。
        /// </summary>
        public static void Until(Func<bool> condiction, TimeSpan cycleInterval)
        {
            var flag = false;
            while (true)
            {
                TimeSpan time = ExecuteTime.Measure(() => flag = condiction());
                if (flag) return;
                if (time < cycleInterval) Thread.Sleep(cycleInterval - time);
            }
        }
    }
}