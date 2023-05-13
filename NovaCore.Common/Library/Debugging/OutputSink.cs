using System;
using System.IO;
using System.Runtime.InteropServices;

namespace NovaCore.Common.Debugging;

// https://stackoverflow.com/questions/1412288/suppress-3rd-party-library-console-output
internal class OutputSink : IDisposable
{
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    public static extern int SetStdHandle(int nStdHandle, IntPtr hHandle);

    private readonly TextWriter oldOut;
    private readonly TextWriter oldError;
    private readonly IntPtr oldOutHandle;
    private readonly IntPtr oldErrorHandle;

    public OutputSink()
    {
        oldOutHandle = GetStdHandle(-11);
        oldErrorHandle = GetStdHandle(-12);
        oldOut = Console.Out;
        oldError = Console.Error;
        Console.SetOut(TextWriter.Null);
        Console.SetError(TextWriter.Null);
        SetStdHandle(-11, IntPtr.Zero);
        SetStdHandle(-12, IntPtr.Zero);
    }

    public void Dispose()
    {
        SetStdHandle(-11, oldOutHandle);
        SetStdHandle(-12, oldErrorHandle);
        Console.SetOut(oldOut);
        Console.SetError(oldError);
    }
}