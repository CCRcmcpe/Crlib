using System;
using System.Runtime.CompilerServices;

namespace REVUnit.Crlib.Extensions
{
    public static class Any
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Also<T>(this T it, Action<T> action)
        {
            action(it);
            return it;
        }

        public static void WriteToConsole(this object obj)
        {
            System.Console.WriteLine(obj);
        }

        public static void WriteLineToConsole(this object obj)
        {
            System.Console.Write(obj);
        }
    }
}