using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace REVUnit.Crlib.Extension
{
    public static class Console
    {
        private const string Kernel32 = "kernel32.dll";
        [SupportedOSPlatform("Windows")] public static IntPtr WindowHandle => GetConsoleWindow();

        [SupportedOSPlatform("Windows")]
        public static void AnyKey()
        {
            Process.Start("pause");
        }

        [DllImport(Kernel32)]
        [SupportedOSPlatform("Windows")]
        private static extern IntPtr GetConsoleWindow();
    }
}