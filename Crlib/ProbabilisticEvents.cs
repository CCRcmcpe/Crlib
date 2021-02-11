using System;

namespace REVUnit.Crlib
{
    public class ProbabilisticEvents
    {
        /// <summary>
        ///     此方法有 <paramref name="probability" /> 的概率返回 <c>true</c>，使用 <see cref="System.Random" /> 类。
        /// </summary>
        public static bool HappensProbability(float probability)
        {
            var random = new Random();
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
    }
}