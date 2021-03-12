using System;

namespace REVUnit.Crlib.Extensions
{
    public static class Convertible
    {
        public static object? ToType(this IConvertible? value, Type targetType, IFormatProvider? format = null) =>
            value?.ToType(targetType, format);

        public static T? ToType<T>(this IConvertible? value, IFormatProvider? format = null)
        {
            object? o = value?.ToType(typeof(T), format);
            return o != null ? (T) o : default;
        }

        public static bool TryToType<T>(this IConvertible? value, out T? result, IFormatProvider? format = null)
        {
            try
            {
                result = value.ToType<T>();
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}