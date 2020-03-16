using System;
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
    }
}