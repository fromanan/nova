using System;
using System.Linq;
using NovaCore.Common.Logging;

namespace NovaCore.Common.Debugging
{
    public static partial class Debug
    {
        public static void Log(string message, ConsoleColor color = Output.DEFAULT_TEXT_COLOR)
        {
            ConsoleColor oldColor = Output.SetTextColor(color);
            PrimaryChannel.Write(LogLevel.Log, message);
            Output.SetTextColor(oldColor);
        }
        
        public static void Log(object message, ConsoleColor color = Output.DEFAULT_TEXT_COLOR)
        {
            ConsoleColor oldColor = Output.SetTextColor(color);
            PrimaryChannel.Write(LogLevel.Log, message.ToString());
            Output.SetTextColor(oldColor);
        }
        
        public static void Log(IFormattable message, ConsoleColor color = Output.DEFAULT_TEXT_COLOR)
        {
            ConsoleColor oldColor = Output.SetTextColor(color);
            PrimaryChannel.Write(LogLevel.Log, message);
            Output.SetTextColor(oldColor);
        }
        
        public static void Log(params string[] messages)
        {
            PrimaryChannel.Write(LogLevel.Log, string.Join("\n", messages));
        }

        public static void Log(params IFormattable[] messages)
        {
            PrimaryChannel.Write(LogLevel.Log, string.Join("\n", messages.Select(s => s.ToString())));
        }

        public static void LogInfo(string message)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.InfoColor);
            PrimaryChannel.Write(LogLevel.Info, message);
            Output.SetTextColor(oldColor);
        }
        
        public static void LogInfo(IFormattable message)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.InfoColor);
            PrimaryChannel.Write(LogLevel.Info, message);
            Output.SetTextColor(oldColor);
        }

        public static void LogWarning(string message)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.WarningColor);
            PrimaryChannel.Write(LogLevel.Warning, message);
            Output.SetTextColor(oldColor);
        }
        
        public static void LogWarning(IFormattable message)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.WarningColor);
            PrimaryChannel.Write(LogLevel.Warning, message);
            Output.SetTextColor(oldColor);
        }
        
        public static void LogError(string message)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.ErrorColor);
            PrimaryChannel.Write(LogLevel.Error, message);
            Output.SetTextColor(oldColor);
        }
        
        public static void LogError(IFormattable message)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.ErrorColor);
            PrimaryChannel.Write(LogLevel.Error, message);
            Output.SetTextColor(oldColor);
        }
        
        public static void LogException(string message, bool exit = false)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.ExceptionColor);
            PrimaryChannel.Write(LogLevel.Exception, message);
            if (exit)
            {
                Global.Exit(ExitCode.Error);
            }
            Output.SetTextColor(oldColor);
        }
        
        public static void LogException(IFormattable message, bool exit = false)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.ExceptionColor);
            PrimaryChannel.Write(LogLevel.Exception, message);
            if (exit)
            {
                Global.Exit(ExitCode.Error);
            }
            Output.SetTextColor(oldColor);
        }
        
        public static void LogException(Exception exception, bool exit = false)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.ExceptionColor);
            PrimaryChannel.Write(LogLevel.Exception, exception.Message);
            if (exit)
            {
                Global.Exit(ExitCode.Error);
            }
            Output.SetTextColor(oldColor);
        }
        
        public static void LogCritical(string message, bool exit = false)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.CriticalColor);
            PrimaryChannel.Write(LogLevel.Critical, message);
            if (exit)
            {
                Global.Exit(ExitCode.Critical);
            }
            Output.SetTextColor(oldColor);
        }
        
        public static void LogCritical(IFormattable message, bool exit)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.CriticalColor);
            PrimaryChannel.Write(LogLevel.Critical, message);
            if (exit)
            {
                Global.Exit(ExitCode.Critical);
            }
            Output.SetTextColor(oldColor);
        }
        
        public static void LogCrash(string message)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.CrashColor);
            PrimaryChannel.Write(LogLevel.Crash, message);
            Global.Exit(ExitCode.Crash);
            Output.SetTextColor(oldColor);
        }
        
        public static void LogCrash(IFormattable message)
        {
            ConsoleColor oldColor = Output.SetTextColor(Output.ChannelPalette.CrashColor);
            PrimaryChannel.Write(LogLevel.Crash, message);
            Global.Exit(ExitCode.Crash);
            Output.SetTextColor(oldColor);
        }

        public static void LogCustom(string tag, string message, ConsoleColor color = Output.DEFAULT_TEXT_COLOR)
        {
            ConsoleColor oldColor = Output.SetTextColor(color);
            PrimaryChannel.Write(LogLevel.Custom, message, tag.ToUpper());
            Output.SetTextColor(oldColor);
        }
        
        public static void LogCustom(string tag, IFormattable message, ConsoleColor color = Output.DEFAULT_TEXT_COLOR)
        {
            ConsoleColor oldColor = Output.SetTextColor(color);
            PrimaryChannel.Write(LogLevel.Custom, message, tag.ToUpper());
            Output.SetTextColor(oldColor);
        }
    }
}