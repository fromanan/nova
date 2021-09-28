using System;

namespace NovaCore.Library.Logging
{
    public static partial class Debug
    {
        public static void Log(string message)
        {
            Output.WriteLine(message);
        }
        
        public static void Log(params string[] messages)
        {
            Output.WriteLine(string.Join("\n", messages));
        }

        public static void LogInfo(string message)
        {
            Output.WriteLine($"[INFO]: {message}");
        }
        
        public static void LogWarning(string message)
        {
            Output.WriteLine($"[WARNING]: {message}");
        }
        
        public static void LogError(string message)
        {
            Output.WriteLine($"[ERROR]: {message}");
        }
        
        public static void LogException(string message, bool exit = false)
        {
            Output.WriteLine($"[EXCEPTION]: {message}");
            if (exit)
            {
                Program.Exit(ExitCode.Error);
            }
        }
        
        public static void LogCritical(string message, bool exit = false)
        {
            Output.WriteLine($"[CRITICAL]: {message}");
            if (exit)
            {
                Program.Exit(ExitCode.Critical);
            }
        }
        
        public static void LogCrash(string message)
        {
            Output.WriteLine($"[CRASH]: {message}");
            Program.Exit(ExitCode.Crash);
        }

        public static void LogCustom(string tag, string message)
        {
            Output.WriteLine($"[{tag.ToUpper()}]: {message}");
        }
        
        public static void Log(IFormattable message)
        {
            Output.WriteLine(message);
        }
        
        public static void LogInfo(IFormattable message)
        {
            Output.WriteLine($"[INFO]: {message}");
        }
        
        public static void LogWarning(IFormattable message)
        {
            Output.WriteLine($"[WARNING]: {message}");
        }
        
        public static void LogError(IFormattable message)
        {
            Output.WriteLine($"[ERROR]: {message}");
        }
        
        public static void LogException(IFormattable message, bool exit = false)
        {
            Output.WriteLine($"[EXCEPTION]: {message}");
            if (exit)
            {
                Program.Exit(ExitCode.Error);
            }
        }
        
        public static void LogCritical(IFormattable message, bool exit)
        {
            Output.WriteLine($"[CRITICAL]: {message}");
            if (exit)
            {
                Program.Exit(ExitCode.Critical);
            }
        }
        
        public static void LogCrash(IFormattable message)
        {
            Output.WriteLine($"[CRASH]: {message}");
            Program.Exit(ExitCode.Crash);
        }
        
        public static void LogCustom(string tag, IFormattable message)
        {
            Output.WriteLine($"[{tag.ToUpper()}]: {message}");
        }

        public static void Log(object message)
        {
            Output.WriteLine(message);
        }
        
        public static void LogException(Exception exception, bool exit = false)
        {
            Output.WriteLine($"[EXCEPTION]: {exception.Message}");
            ExceptionStack.Push(exception);
            if (exit)
            {
                Program.Exit(ExitCode.Error);
            }
        }
    }
}