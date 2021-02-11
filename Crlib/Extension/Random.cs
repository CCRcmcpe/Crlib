using System;
using System.Collections.Generic;
using System.Text;

namespace REVUnit.Crlib.Extension
{
    public static class Random
    {
        public static T GetFromList<T>(this System.Random random, IList<T> list)
        {
            return list[random.Next(list.Count)];
        }

        public static string NextString(this System.Random random, char start, char end, int length)
        {
            var stringBuilder = new StringBuilder(length);
            for (var i = 0; i < length; i++) stringBuilder.Append((char) random.Next(start, end));
            return stringBuilder.ToString();
        }
    }
}