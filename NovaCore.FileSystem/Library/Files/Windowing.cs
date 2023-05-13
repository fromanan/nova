using System;
using System.IO;
using System.Runtime.InteropServices;

namespace NovaCore.Files;

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
    //public static extern bool AttachConsole(uint dwProcessId);
    public static extern bool AttachConsole(int pid);
        
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetConsoleWindow();
        
    [DllImport("kernel32.dll", SetLastError=true, ExactSpelling=true)]
    public static extern bool FreeConsole();
        
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    public static extern int SetStdHandle(int nStdHandle, IntPtr hHandle);
        
    public static void CreateConsole()
    {
        AllocConsole();

        // stdout's handle seems to always be equal to 7
        IntPtr defaultStdout = new(7);
        IntPtr currentStdout = GetStdHandle(STD_OUTPUT_HANDLE);

        // reset stdout
        if (currentStdout != defaultStdout)
        {
            SetStdHandle(STD_OUTPUT_HANDLE, defaultStdout);
        }

        // reopen stdout
        TextWriter writer = new StreamWriter(Console.OpenStandardOutput())
        {
            AutoFlush = true
        };
        Console.SetOut(writer);
    }

    // P/Invoke required:
    public const uint STD_OUTPUT_HANDLE = 0xFFFFFFF5;
        
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetStdHandle(uint nStdHandle);
        
    [DllImport("kernel32.dll")]
    public static extern void SetStdHandle(uint nStdHandle, IntPtr handle);
        
    [DllImport("kernel32")]
    public static extern bool AllocConsole();
}