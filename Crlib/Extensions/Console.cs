using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace REVUnit.Crlib.Extensions
{
    public static class Console
    {
        private const string Kernel32 = "kernel32.dll";
        [SupportedOSPlatform("Windows")] public static IntPtr WindowHandle => GetConsoleWindow();

        [DllImport(Kernel32)]
        [SupportedOSPlatform("Windows")]
        private static extern IntPtr GetConsoleWindow();
    }
}