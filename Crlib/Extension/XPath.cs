using System;
using System.IO;
using System.Linq;

namespace REVUnit.Crlib.Extension
{
    public static class XPath
    {
        public static string? ExpandPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value cannot be null or whitespace", nameof(path));
            return Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.Machine)
                ?.Split(';')
                .Select(p => Path.Combine(p, path)).Where(File.Exists).FirstOrDefault();
        }

        public static bool IsDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value cannot be null or whitespace", nameof(path));
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }
    }
}