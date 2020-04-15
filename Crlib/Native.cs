using System;
using System.Runtime.InteropServices;

namespace REVUnit.Crlib
{
    public delegate bool ConsoleCtrlHandler(uint ctrlType);

    internal static class Native
    {
#if WINDOWS
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandler HandlerRoutine,
            bool add);
#endif
    }
}