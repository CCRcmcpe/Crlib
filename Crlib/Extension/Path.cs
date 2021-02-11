using System;
using System.IO;
using REVUnit.Crlib.Properties;

namespace REVUnit.Crlib.Extension
{
    public static class Path
    {
        public static bool IsDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(Resources.XPath_Exception_PathEmpty, nameof(path));
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }
    }
}