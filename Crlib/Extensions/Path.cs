using System;
using System.IO;

namespace REVUnit.Crlib.Extensions
{
    public static class Path
    {
        public static bool IsDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentOutOfRangeException(nameof(path));
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }
    }
}