using System;
using System.Collections.Generic;
using System.Text;

namespace REVUnit.Crlib.Extension
{
    public static class XRandom
    {
        public static string NextString(this Random random, char start, char end, int length)
        {
            var stringBuilder = new StringBuilder(length);
            for (var i = 0; i < length; i++) stringBuilder.Append((char) random.Next(start, end));
            return stringBuilder.ToString();
        }

        public static T GetFromList<T>(this Random random, IList<T> list)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            if (list == null) throw new ArgumentNullException(nameof(list));
            return list[random.Next(list.Count)];
        }
    }
}