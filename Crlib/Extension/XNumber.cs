using System;
using System.Collections.Generic;
using System.Numerics;

namespace REVUnit.Crlib.Extension
{
    public static class XNumber
    {
        public static bool AtRange(this int i, int min, int max) => i >= min && i <= max;

        public static string Based(this int i, int @base) => Convert.ToString(i, @base);

        public static int[] Digits(this int value)
        {
            var stack = new Stack<int>();
            while (value > 0)
            {
                stack.Push(value % 10);
                value /= 10;
            }

            return stack.ToArray();
        }

        public static BigInteger[] GetFactors(this BigInteger i)
        {
            var list = new List<BigInteger>();
            BigInteger bigInteger = 2;
            while (i > 1L)
            {
                while (i % bigInteger == 0L)
                {
                    i /= bigInteger;
                    list.Add(bigInteger);
                }

                bigInteger = ++bigInteger;
            }

            return list.ToArray();
        }

        public static bool InRange(this int i, int min, int max) => i > min && i < max;
    }
}