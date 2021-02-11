using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace REVUnit.Crlib.Extension
{
    public static class Console
    {
        static Console()
        {
            if (!OperatingSystem.IsWindows()) return;
            if (!SetConsoleCtrlHandler(CallExiting, true))
            {
                throw new Exception("Unable to set ConsoleCtrlHandler");
            }
        }

        [SupportedOSPlatform("Windows")] public static IntPtr WindowHandle => GetConsoleWindow();

        [SupportedOSPlatform("Windows")]
        public static void AnyKey()
        {
            Process.Start("pause");
        }

        [SupportedOSPlatform("Windows")]
        private static bool CallExiting(CtrlType ctrlType)
        {
            Exiting?.Invoke(ctrlType);
            return true;
        }

        [SupportedOSPlatform("Windows")]
        public static event Action<CtrlType>? Exiting; // ReSharper disable InconsistentNaming

        private const string Kernel32 = "kernel32.dll";

        [DllImport(Kernel32)]
        [SupportedOSPlatform("Windows")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport(Kernel32)]
        [SupportedOSPlatform("Windows")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandler handlerRoutine,
                                                         bool add);

        public delegate bool ConsoleCtrlHandler(CtrlType ctrlType);

        /// <summary>
        ///     Enumerated type for the control messages sent to the handler routine
        /// </summary>
        public enum CtrlType : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }
    }
}