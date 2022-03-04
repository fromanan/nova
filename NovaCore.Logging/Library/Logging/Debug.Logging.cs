using System;
using System.Linq;

namespace NovaCore.Logging
{
    public static partial class Debug
    {
        public enum LogType
        {
            Log, 
            Info, 
            Warning, 
            Error, 
            Exception, 
            Critical, 
            Crash,
            Custom
        }

        public static void Log(string message, ConsoleColor color = DEFAULT_TEXT_COLOR)
        {
            using (new ColorPreset(color))
            {
                PrimaryChannel.Write(LogType.Log, message);
            }
        }
        
        public static void Log(object message, ConsoleColor color = DEFAULT_TEXT_COLOR)
        {
            using (new ColorPreset(color))
            {
                PrimaryChannel.Write(LogType.Log, message.ToString());
            }
        }
        
        public static void Log(IFormattable message, ConsoleColor color = DEFAULT_TEXT_COLOR)
        {
            using (new ColorPreset(color))
            {
                PrimaryChannel.Write(LogType.Log, message);
            }
        }
        
        public static void Log(params string[] messages)
        {
            PrimaryChannel.Write(LogType.Log, string.Join("\n", messages));
        }

        public static void Log(params IFormattable[] messages)
        {
            PrimaryChannel.Write(LogType.Log, string.Join("\n", messages.Select(s => s.ToString())));
        }

        public static void LogInfo(string message)
        {
            using (new ColorPreset(ChannelPalette.InfoColor))
            {
                PrimaryChannel.Write(LogType.Info, message);
            }
        }
        
        public static void LogInfo(IFormattable message)
        {
            using (new ColorPreset(ChannelPalette.InfoColor))
            {
                PrimaryChannel.Write(LogType.Info, message);
            }
        }

        public static void LogWarning(string message)
        {
            using (new ColorPreset(ChannelPalette.WarningColor))
            {
                PrimaryChannel.Write(LogType.Warning, message);
            }
        }
        
        public static void LogWarning(IFormattable message)
        {
            using (new ColorPreset(ChannelPalette.WarningColor))
            {
                PrimaryChannel.Write(LogType.Warning, message);
            }
        }
        
        public static void LogError(string message)
        {
            using (new ColorPreset(ChannelPalette.ErrorColor))
            {
                PrimaryChannel.Write(LogType.Error, message);
            }
        }
        
        public static void LogError(IFormattable message)
        {
            using (new ColorPreset(ChannelPalette.ErrorColor))
            {
                PrimaryChannel.Write(LogType.Error, message);
            }
        }
        
        public static void LogException(string message, bool exit = false)
        {
            using (new ColorPreset(ChannelPalette.ExceptionColor))
            {
                PrimaryChannel.Write(LogType.Exception, message);
                if (exit)
                {
                    NovaApplication.Exit(ExitCode.Error);
                }
            }
        }
        
        public static void LogException(IFormattable message, bool exit = false)
        {
            using (new ColorPreset(ChannelPalette.ExceptionColor))
            {
                PrimaryChannel.Write(LogType.Exception, message);
                if (exit)
                {
                    NovaApplication.Exit(ExitCode.Error);
                }
            }
        }
        
        public static void LogException(Exception exception, bool exit = false)
        {
            using (new ColorPreset(ChannelPalette.ExceptionColor))
            {
                PrimaryChannel.Write(LogType.Exception, exception.Message);
                if (exit)
                {
                    NovaApplication.Exit(ExitCode.Error);
                }
            }
        }
        
        public static void LogCritical(string message, bool exit = false)
        {
            using (new ColorPreset(ChannelPalette.CriticalColor))
            {
                PrimaryChannel.Write(LogType.Critical, message);
                if (exit)
                {
                    NovaApplication.Exit(ExitCode.Critical);
                }
            }
        }
        
        public static void LogCritical(IFormattable message, bool exit)
        {
            using (new ColorPreset(ChannelPalette.CriticalColor))
            {
                PrimaryChannel.Write(LogType.Critical, message);
                if (exit)
                {
                    NovaApplication.Exit(ExitCode.Critical);
                }
            }
        }
        
        public static void LogCrash(string message)
        {
            using (new ColorPreset(ChannelPalette.CrashColor))
            {
                PrimaryChannel.Write(LogType.Crash, message);
                NovaApplication.Exit(ExitCode.Crash);
            }
        }
        
        public static void LogCrash(IFormattable message)
        {
            using (new ColorPreset(ChannelPalette.CrashColor))
            {
                PrimaryChannel.Write(LogType.Crash, message);
                NovaApplication.Exit(ExitCode.Crash);
            }
        }

        public static void LogCustom(string tag, string message, ConsoleColor color = DEFAULT_TEXT_COLOR)
        {
            using (new ColorPreset(color))
            {
                PrimaryChannel.Write(LogType.Custom, message, tag.ToUpper());
            }
        }
        
        public static void LogCustom(string tag, IFormattable message, ConsoleColor color = DEFAULT_TEXT_COLOR)
        {
            using (new ColorPreset(color))
            {
                PrimaryChannel.Write(LogType.Custom, message, tag.ToUpper());
            }
        }
    }
}