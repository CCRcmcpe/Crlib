#if WINDOWS
using System;
using System.Runtime.InteropServices;
#endif

namespace REVUnit.Crlib
{
    internal static class Native
    {
#if WINDOWS
        public delegate bool ConsoleCtrlHandler(uint ctrlType);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandler HandlerRoutine,
            bool add);
#endif
    }
}