using System;
using System.Runtime.InteropServices;

namespace NovaCore.Files
{
    public static class Windowing
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // When you don't want the ProcessId, use this overload and pass IntPtr.Zero for the second parameter
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        /// <summary>The GetForegroundWindow function returns a handle to the foreground window.</summary>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(HandleRef hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint="FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AttachConsole(uint dwProcessId);
        
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        
        [DllImport("kernel32.dll", SetLastError=true, ExactSpelling=true)]
        public static extern bool FreeConsole();
    }
}