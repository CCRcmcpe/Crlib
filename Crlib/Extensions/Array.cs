namespace REVUnit.Crlib.Extensions
{
    public static class Array
    {
        public static T[][] Create<T>(int dim0Len, int dim1Len)
        {
            var array = new T[dim0Len][];

            for (var x = 0; x < array.Length; x++) array[x] = new T[dim1Len];

            return array;
        }
    }
}