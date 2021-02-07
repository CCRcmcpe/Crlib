using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace REVUnit.Crlib
{
    internal static class Native
    {
        public delegate bool ConsoleCtrlHandler(uint ctrlType);

        [SupportedOSPlatform("Windows")]
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [SupportedOSPlatform("Windows")]
        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandler handlerRoutine,
                                                        bool add);
    }
}