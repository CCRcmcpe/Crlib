﻿using System;
using System.Linq;
using REVUnit.Crlib.Extensions;
using REVUnit.Crlib.Input.Properties;
using Console = System.Console;

namespace REVUnit.Crlib.Input
{
    public class Cin : Scanner
    {
        public delegate Exception? Parser<T>(string value, out T? result);

        public Cin() : base(Console.In, Environment.NewLine) { }

        public bool WriteEnumDescription { get; set; } = true;
        public bool ThrowOnUndefinedEnum { get; set; } = true;
        public Exception? LastException { get; private set; }

        public T? Get<T>(string? hint = null) where T : IConvertible => Get(hint, (Parser<T>) TryParse);

        public T? Get<T>(string? hint, Parser<T> parser)
        {
            bool interactive = Environment.UserInteractive;

            if (string.IsNullOrWhiteSpace(hint))
                hint = string.Empty;
            else if (!hint.EndsWith(": "))
                hint += ": ";

            Type type = typeof(T);
            if (type.IsEnum && interactive && WriteEnumDescription)
            {
                Console.WriteLine(GetEnumDescription(type));
            }

            while (true)
            {
                if (interactive) Console.Write(hint);
                string? token = NextToken();
                if (token == null) return default;

                LastException = parser(token, out T? result);
                return LastException == null ? result : default;
            }
        }

        private static string GetEnumDescription(Type enumType)
        {
            return enumType.GetEnumValues().Cast<IConvertible>()
                           .Select(v => v.ToType(enumType.GetEnumUnderlyingType())!)
                           .Select(v => $"[{v}]={enumType.GetEnumName(v)}").GetLiteral();
        }

        private Exception? TryParse<T>(string value, out T? result) where T : IConvertible
        {
            result = default;
            Type type = typeof(T);

            try
            {
                if (!type.IsEnum)
                {
                    object? o = result.ToType(type);
                    result = o != null ? (T?) o : default;
                }

                result = (T) Enum.Parse(type, value, IgnoreCase);
            }
            catch (Exception e)
            {
                return e;
            }

            if (ThrowOnUndefinedEnum && !Enum.IsDefined(type, result))
            {
                throw new InvalidTokenException(string.Format(Resources.Cin_InputOutOfRange, type));
            }

            return null;
        }
    }
}