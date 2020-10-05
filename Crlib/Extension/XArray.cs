using System.Linq;

namespace REVUnit.Crlib.Extension
{
    public static class XArray
    {
        public static T[][] Create<T>(int dim0Len, int dim1Len)
        {
            var array = new T[dim0Len][];

            for (var x = 0; x < array.Length; x++) array[x] = new T[dim1Len];

            return array;
        }

        public static T[] Of<T>(params T[] elements) => elements;

        public static T[] With<T>(this T obj, params T[] elements) => elements.Prepend(obj).ToArray();
    }
}