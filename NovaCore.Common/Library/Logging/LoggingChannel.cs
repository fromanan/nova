using System;
using System.IO;
using System.Linq;
using NovaCore.Common.Extensions;

namespace NovaCore.Common.Logging
{
    public class LoggingChannel : IDisposable
    {
        public ChannelPalette Palette { get; set; }= new();
        
        public TextWriter Output = TextWriter.Null;
        public TextWriter Info = TextWriter.Null;
        public TextWriter Warning = TextWriter.Null;
        public TextWriter Error = TextWriter.Null;
        public TextWriter Exception = TextWriter.Null;
        public TextWriter Critical = TextWriter.Null;
        public TextWriter Crash = TextWriter.Null;
        public TextWriter Custom = TextWriter.Null;
        
        private static string FormatLog(string header, string message) => $"[{header.ToUpper()}] {message}";
        
        private static string FormatLog(string header, IFormattable message) => $"[{header.ToUpper()}] {message}";

        public LoggingChannel()
        {
            RedirectAll(Common.Output.StandardOutput);
        }
        
        public LoggingChannel(TextWriter writer)
        {
            RedirectAll(writer);
        }

        public LoggingChannel(string filepath)
        {
            WriteAllToFile(filepath);
        }

        public void Log(string message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.DefaultColor);
            Output.WriteLine(message);
            Common.Output.SetTextColor(oldColor);
        }
        
        public void Log(string message, ConsoleColor color)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(color);
            Output.WriteLine(message);
            Common.Output.SetTextColor(oldColor);
        }
        
        public void Log(object message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.DefaultColor);
            Output.WriteLine(message);
            Common.Output.SetTextColor(oldColor);
        }
        
        public void Log(IFormattable message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.DefaultColor);
            Output.WriteLine(message);
            Common.Output.SetTextColor(oldColor);
        }
        
        public void Log(params string[] messages)
        {
            Output.WriteLine(messages.Merge("\n"));
        }

        public void Log(params IFormattable[] messages)
        {
            Output.WriteLine(messages.Select(s => s.ToString()).Merge("\n"));
        }
        
        public void LogText(string message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.DefaultColor);
            Output.Write(message);
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogText(string message, ConsoleColor color)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.DefaultColor);
            Output.Write(message);
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogText(object message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.DefaultColor);
            Output.Write(message);
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogText(IFormattable message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.DefaultColor);
            Output.Write(message);
            Common.Output.SetTextColor(oldColor);
        }

        public void LogInfo(string message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.InfoColor);
            Info.WriteLine(FormatLog("INFO", message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogInfo(IFormattable message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.InfoColor);
            Info.WriteLine(FormatLog("INFO", message));
            Common.Output.SetTextColor(oldColor);
        }

        public void LogWarning(string message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.WarningColor);
            Warning.WriteLine(FormatLog("WARNING", message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogWarning(IFormattable message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.WarningColor);
            Warning.WriteLine(FormatLog("WARNING", message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogError(string message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.ErrorColor);
            Error.WriteLine(FormatLog("ERROR", message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogError(IFormattable message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.ErrorColor);
            Error.WriteLine(FormatLog("ERROR", message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogException(string message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.ExceptionColor);
            Exception.WriteLine(FormatLog("EXCEPTION", message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogException(IFormattable message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.ExceptionColor);
            Exception.WriteLine(FormatLog("EXCEPTION", message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogException(Exception exception)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.ExceptionColor);
            Exception.WriteLine(FormatLog("EXCEPTION", exception.Message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogCritical(string message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.CriticalColor);
            Critical.WriteLine(FormatLog("CRITICAL", message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogCritical(IFormattable message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.CriticalColor);
            Critical.WriteLine(FormatLog("CRITICAL", message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogCrash(string message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.CrashColor);
            Crash.WriteLine(FormatLog("CRASH", message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogCrash(IFormattable message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.CrashColor);
            Crash.WriteLine(FormatLog("CRASH", message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogCustom(string tag, string message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.DefaultColor);
            Custom.WriteLine(FormatLog(tag.ToUpper(), message));
            Common.Output.SetTextColor(oldColor);
        }

        public void LogCustom(string tag, string message, ConsoleColor color)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(color);
            Custom.WriteLine(FormatLog(tag.ToUpper(), message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogCustom(string tag, IFormattable message)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(Palette.DefaultColor);
            Custom.WriteLine(FormatLog(tag.ToUpper(), message));
            Common.Output.SetTextColor(oldColor);
        }
        
        public void LogCustom(string tag, IFormattable message, ConsoleColor color)
        {
            ConsoleColor oldColor = Common.Output.SetTextColor(color);
            Custom.WriteLine(FormatLog(tag.ToUpper(), message));
            Common.Output.SetTextColor(oldColor);
        }

        public void SubscribeLogger(Logger logger)
        {
            // Logging
            logger.OnLog += Log;
            logger.OnLogC += Log;
            logger.OnLogF += Log;
            logger.OnLogD += Log;
            
            // Text
            logger.OnLogText += LogText;
            logger.OnLogTextC += LogText;
            logger.OnLogTextF += LogText;
            logger.OnLogTextD += LogText;
            
            // Info
            logger.OnLogInfo += LogInfo;
            logger.OnLogInfoF += LogInfo;
            
            // Warning
            logger.OnLogWarning += LogWarning;
            logger.OnLogWarningF += LogWarning;
            
            // Error
            logger.OnLogError += LogError;
            logger.OnLogErrorF += LogError;
            
            // Exception
            logger.OnLogException += LogException;
            logger.OnLogExceptionF += LogException;
            logger.OnLogExceptionE += LogException;
            
            // Critical
            logger.OnLogCritical += LogCritical;
            logger.OnLogCriticalF += LogCritical;
            
            // Crash
            logger.OnLogCrash += LogCrash;
            logger.OnLogCrashF += LogCrash;
            
            // Custom
            logger.OnLogCustom += LogCustom;
            logger.OnLogCustomC += LogCustom;
            logger.OnLogCustomF += LogCustom;
            logger.OnLogCustomFC += LogCustom;
        }
        
        public void UnsubscribeLogger(Logger logger)
        {
            // Logging
            logger.OnLog -= Log;
            logger.OnLogC -= Log;
            logger.OnLogF -= Log;
            logger.OnLogD -= Log;
            
            // Info
            logger.OnLogInfo -= LogInfo;
            logger.OnLogInfoF -= LogInfo;
            
            // Warning
            logger.OnLogWarning -= LogWarning;
            logger.OnLogWarningF -= LogWarning;
            
            // Error
            logger.OnLogError -= LogError;
            logger.OnLogErrorF -= LogError;
            
            // Exception
            logger.OnLogException -= LogException;
            logger.OnLogExceptionF -= LogException;
            logger.OnLogExceptionE -= LogException;
            
            // Critical
            logger.OnLogCritical -= LogCritical;
            logger.OnLogCriticalF -= LogCritical;
            
            // Crash
            logger.OnLogCrash -= LogCrash;
            logger.OnLogCrashF -= LogCrash;
            
            // Custom
            logger.OnLogCustom -= LogCustom;
            logger.OnLogCustomC -= LogCustom;
            logger.OnLogCustomF -= LogCustom;
            logger.OnLogCustomFC -= LogCustom;
        }

        private FileStream FileHandle;
        private StreamWriter StreamWriter;
        
        public void Redirect(LogLevel logLevel, TextWriter writer)
        {
            switch (logLevel)
            {
                case LogLevel.Log:
                    Output = writer;
                    break;
                case LogLevel.Info:
                    Info = writer;
                    break;
                case LogLevel.Warning:
                    Warning = writer;
                    break;
                case LogLevel.Error:
                    Error = writer;
                    break;
                case LogLevel.Exception:
                    Exception = writer;
                    break;
                case LogLevel.Critical:
                    Critical = writer;
                    break;
                case LogLevel.Crash:
                    Crash = writer;
                    break;
                case LogLevel.Custom:
                    Custom = writer;
                    break;
            }
        }

        public void RedirectAll(TextWriter writer)
        {
            Output = writer;
            Info = writer;
            Warning = writer;
            Error = writer;
            Exception = writer;
            Critical = writer;
            Crash = writer;
            Custom = writer;
        }

        public void WriteAllToFile(string filepath)
        {
            FileHandle = new FileStream(filepath, FileMode.Append, FileAccess.Write);
            StreamWriter = new StreamWriter(FileHandle);
            RedirectAll(StreamWriter);
        }

        public void Close()
        {
            RedirectAll(TextWriter.Null);
            StreamWriter.Close();
            FileHandle.Close();
        }

        public void Dispose()
        {
            Close();
        }
        
        public void Write(LogLevel logLevel, string message, string tag = "")
        {
            switch (logLevel)
            {
                case LogLevel.Log:
                    Output.WriteLine(message);
                    break;
                case LogLevel.Info:
                    Info.WriteLine(FormatLog("INFO", message));
                    break;
                case LogLevel.Warning:
                    Warning.WriteLine(FormatLog("WARNING", message));
                    break;
                case LogLevel.Error:
                    Error.WriteLine(FormatLog("ERROR", message));
                    break;
                case LogLevel.Exception:
                    Exception.WriteLine(FormatLog("EXCEPTION", message));
                    break;
                case LogLevel.Critical:
                    Critical.WriteLine(FormatLog("CRITICAL", message));
                    break;
                case LogLevel.Crash:
                    Crash.WriteLine(FormatLog("CRASH", message));
                    break;
                case LogLevel.Custom:
                    Custom.WriteLine(FormatLog(tag, message));
                    break;
            }
        }

        public void Write(LogLevel logLevel, IFormattable message, string tag = "")
        {
            Write(logLevel, message.ToString(), tag);
        }
    }
}