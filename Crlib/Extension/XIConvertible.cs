using System;

namespace REVUnit.Crlib.Extension
{
    public static class XIConvertible
    {
        public static object ToType(this IConvertible value, Type targetType, IFormatProvider format = null)
        {
            return value.ToType(targetType, format);
        }

        public static T ToType<T>(this IConvertible value, IFormatProvider format = null)
        {
            return (T) value.ToType(typeof(T), format);
        }

        public static bool TryToType<T>(this IConvertible value,
            out T target, IFormatProvider format = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            try
            {
                target = value.ToType<T>(format);
            }
            catch
            {
                target = default;
                return false;
            }

            return true;
        }
    }
}