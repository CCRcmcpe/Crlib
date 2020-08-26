using System;
using System.IO;

namespace REVUnit.Crlib.Extension
{
    public static class XPath
    {
        public static bool IsDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value cannot be null or whitespace", nameof(path));
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }
    }
}