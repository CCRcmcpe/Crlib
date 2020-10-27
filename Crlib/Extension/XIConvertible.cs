using System;
using System.Diagnostics.CodeAnalysis;

namespace REVUnit.Crlib.Extension
{
    public static class XIConvertible
    {
        public static object ToType(this IConvertible value, Type targetType, IFormatProvider? format = null) =>
            value.ToType(targetType, format);

        public static T ToType<T>(this IConvertible value, IFormatProvider? format = null) =>
            (T) value.ToType(typeof(T), format);

        public static bool TryToType<T>(this IConvertible? value,
                                        [MaybeNullWhen(false)] out T target, IFormatProvider? format = null)
        {
            try
            {
                target = value!.ToType<T>(format);
            }
            catch
            {
                target = default!;
                return false;
            }

            return true;
        }
    }
}