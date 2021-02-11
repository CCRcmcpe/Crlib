using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace REVUnit.Crlib.Extension
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

        public static T? ParseOrDefault<TSrc, T>(this TSrc it, TryParser<TSrc, T>.Agent parser) =>
            parser(it, out T? result) ? result : default;

        public static void PopulateWithJson(this object obj, string json)
        {
            JsonConvert.PopulateObject(json, obj);
        }

        public static void PopulateWithJsonFile(this object obj, string filePath)
        {
            JsonConvert.PopulateObject(File.ReadAllText(filePath), obj);
        }

        public static string SerializeToJson(this object obj, Formatting format = Formatting.Indented) =>
            JsonConvert.SerializeObject(obj, format);

        public static void SerializeToJsonFile(this object obj, string filePath,
                                               Formatting format = Formatting.Indented)
        {
            using var jsonTextWriter = new JsonTextWriter(new StreamWriter(filePath, false, Encoding.Default));
            new JsonSerializer { Formatting = format }.Serialize(jsonTextWriter, obj);
        }
    }
}