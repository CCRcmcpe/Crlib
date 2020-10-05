using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace REVUnit.Crlib.Extension
{
    public static class XType
    {
        public static Type? GetType(string typeName, bool ignoreCase = false, bool throwOnError = false)
        {
            Type? type = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(t => t.Name.Equals(typeName,
                    ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
            if (type != null) return type;

            if (throwOnError) throw new Exception("Type not found");

            return null;
        }

        [return: MaybeNull]
        public static T New<T>(this Type type, params object[] parameters)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return (T) Activator.CreateInstance(type, parameters)!;
        }

        public static bool TypeIs(this Type a, Type b) => b.IsAssignableFrom(a);
    }
}