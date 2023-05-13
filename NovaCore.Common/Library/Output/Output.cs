using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NovaCore.Common.Debugging;
using NovaCore.Common.Utilities;

namespace NovaCore.Common;

public static partial class Output
{
    public static TextWriter StandardOutput { get; private set; } = Console.Out;
        
    public static TextWriter StandardError { get; private set; } = Console.Error;
        
    private static bool _redirected;
        
    private static readonly Stack<Exception> ExceptionStack = new();
        
    private static StreamWriter _exceptionWriter;
        
    public static void WriteLine(string message)
    {
        StandardOutput.WriteLine(message);
    }
        
    public static void WriteLine(string message, ConsoleColor color)
    {
        ConsoleColor oldColor = SetTextColor(color);
        StandardOutput.WriteLine(message);
        SetTextColor(oldColor);
    }

    public static void WriteLine(IFormattable message)
    {
        StandardOutput.WriteLine(message);
    }
        
    public static void WriteLine(IFormattable message, ConsoleColor color)
    {
        ConsoleColor oldColor = SetTextColor(color);
        StandardOutput.WriteLine(message);
        SetTextColor(oldColor);
    }
        
    public static async Task WriteLineAsync(string message)
    {
        await StandardOutput.WriteLineAsync(message);
    }

    public static void Write(string message)
    {
        StandardOutput.Write(message);
    }
        
    public static void Write(string message, ConsoleColor color)
    {
        ConsoleColor oldColor = SetTextColor(color);
        StandardOutput.Write(message);
        SetTextColor(oldColor);
    }
        
    public static void Write(IFormattable message)
    {
        StandardOutput.Write(message);
    }
        
    public static void Write(IFormattable message, ConsoleColor color)
    {
        ConsoleColor oldColor = SetTextColor(color);
        StandardOutput.Write(message);
        SetTextColor(oldColor);
    }

    public static void WriteAsync(string message)
    {
        StandardOutput.WriteAsync(message);
    }
        
    public static void LineBreak()
    {
        StandardOutput.WriteLine();
    }
        
    public static void Clear()
    {
        Console.Clear();
    }
        
    public static ConsoleColor SetTextColor(ConsoleColor color = DEFAULT_TEXT_COLOR)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        return oldColor;
    }

    public static ConsoleColor SetBackgroundColor(ConsoleColor color = DEFAULT_BACKGROUND_COLOR)
    {
        ConsoleColor oldColor = Console.BackgroundColor;
        Console.BackgroundColor = color;
        return oldColor;
    }
        
    public static void Redirect(TextWriter redirectedOutput = null)
    {
        Redirect<TextWriter>(redirectedOutput);
    }

    public static void Redirect<T>(T redirectedOutput = null) where T : TextWriter
    {
        // Reset the Output Stream
        if (redirectedOutput == null)
        {
            Console.SetOut(StandardOutput);
            _redirected = false;
        }
        // Set the Output Stream to the TextWriter specified
        else if (_redirected)
        {
            Debug.LogWarning("Cannot redirect output twice");
        }
        else
        {
            Console.SetOut(redirectedOutput);
            _redirected = true;
        }
    }

    public static T OpenBuffer<T>(bool redirect = true) where T : TextWriter, new()
    {
        T buffer = new();
            
        if (redirect)
        {
            Redirect(buffer);
        }

        return buffer;
    }

    public static void CloseBuffer<T>(T buffer) where T : TextWriter
    {
        buffer.Close();
        Redirect();
    }

    private static string ExceptionHeader =>
        $"[ Exception {Guid.NewGuid()} - {DateTime.Now:MM/dd/yyyy hh:mm:ss.fff tt} ]";

    public static void TrackExceptions()
    {
        // Ensure Log Folder Exists
        if (!Directory.Exists(Paths.Log))
            Directory.CreateDirectory(Paths.Log);

        // Create Exception Log in Files
        string filename = $"exception_{DateTime.Now:yyyyMMddHHmmss}.log";
        string filepath = Path.Combine(Paths.Log, filename);
        _exceptionWriter = File.AppendText(filepath);
            
        // Write the Header
        _exceptionWriter.WriteLine(FileHeader());
            
        // Track First Chance Exceptions
        AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
        {
            if (e?.Exception is null) return;
            ExceptionStack.Push(e.Exception);
            _exceptionWriter.WriteLine(ExceptionHeader);
            _exceptionWriter.WriteLine(e.Exception);
            _exceptionWriter.WriteLine();
        };

        // Track Unhandled Exceptions
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            ExceptionStack.Push(e.ExceptionObject as Exception);
            _exceptionWriter.WriteLine(ExceptionHeader);
            _exceptionWriter.WriteLine(e.ExceptionObject);    
            _exceptionWriter.WriteLine();
        };
    }
        
    public static double ElapsedMilliseconds(DateTime startTime, DateTime? endTime = null)
    {
        return ((endTime ?? DateTime.Now) - startTime).TotalMilliseconds;
    }
        
    public static double ElapsedSeconds(DateTime startTime, DateTime? endTime = null)
    {
        return ((endTime ?? DateTime.Now) - startTime).TotalSeconds;
    }

    private static string FileHeader()
    {
        StringBuilder buffer = new();
        buffer.AppendLine($"{AppInfo.ProductName} Version: {AppInfo.ProductVersion}");
        buffer.AppendLine($"Application started at: {DateTime.Now:G}");
        return buffer.ToString();
    }
        
    public static void Timestamp(DateTime startTime, DateTime? endTime = null)
    {
        StandardOutput.WriteLine($"Time Elapsed: {ElapsedSeconds(startTime, endTime):F2} seconds");
    }

    public static void Separator(bool lineBreak = false)
    {
        string separator = new('=', LINE_LENGTH);
        StandardOutput.WriteLine(lineBreak ? $"{separator}\n" : separator);
    }
}