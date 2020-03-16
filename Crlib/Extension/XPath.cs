using System;
using System.IO;
using System.Linq;

namespace REVUnit.Crlib.Extension
{
    public static class XPath
    {
        public static bool IsDirectory(string path)
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }

        public static string ExpandPath(string path)
        {
            return Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.Machine)
                ?.Split(';')
                .Select(p => Path.Combine(p, path)).Where(File.Exists).FirstOrDefault();
        }
    }
}