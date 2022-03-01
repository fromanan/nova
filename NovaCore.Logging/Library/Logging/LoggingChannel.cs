using System;
using System.IO;
using System.Linq;
using NovaCore.Common;

namespace NovaCore.Logging
{
    public class LoggingChannel : IDisposable
    {
        public ChannelPalette Palette = new();
        
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
            RedirectAll(Debug.StandardOutput);
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
            using (new ColorPreset(Palette.DefaultColor))
            {
                Output.WriteLine(message);
            }
        }
        
        public void Log(object message)
        {
            using (new ColorPreset(Palette.DefaultColor))
            {
                Output.WriteLine(message);
            }
        }
        
        public void Log(IFormattable message)
        {
            using (new ColorPreset(Palette.DefaultColor))
            {
                Output.WriteLine(message);
            }
        }
        
        public void Log(params string[] messages)
        {
            Output.WriteLine(string.Join("\n", messages));
        }

        public void Log(params IFormattable[] messages)
        {
            Output.WriteLine(string.Join("\n", messages.Select(s => s.ToString())));
        }

        public void LogInfo(string message)
        {
            using (new ColorPreset(Palette.InfoColor))
            {
                Info.WriteLine(FormatLog("INFO", message));
            }
        }
        
        public void LogInfo(IFormattable message)
        {
            using (new ColorPreset(Palette.InfoColor))
            {
                Info.WriteLine(FormatLog("INFO", message));
            }
        }

        public void LogWarning(string message)
        {
            using (new ColorPreset(Palette.WarningColor))
            {
                Warning.WriteLine(FormatLog("WARNING", message));
            }
        }
        
        public void LogWarning(IFormattable message)
        {
            using (new ColorPreset(Palette.WarningColor))
            {
                Warning.WriteLine(FormatLog("WARNING", message));
            }
        }
        
        public void LogError(string message)
        {
            using (new ColorPreset(Palette.ErrorColor))
            {
                Error.WriteLine(FormatLog("ERROR", message));
            }
        }
        
        public void LogError(IFormattable message)
        {
            using (new ColorPreset(Palette.ErrorColor))
            {
                Error.WriteLine(FormatLog("ERROR", message));
            }
        }
        
        public void LogException(string message)
        {
            using (new ColorPreset(Palette.ExceptionColor))
            {
                Exception.WriteLine(FormatLog("EXCEPTION", message));
            }
        }
        
        public void LogException(IFormattable message)
        {
            using (new ColorPreset(Palette.ExceptionColor))
            {
                Exception.WriteLine(FormatLog("EXCEPTION", message));
            }
        }
        
        public void LogException(Exception exception)
        {
            using (new ColorPreset(Palette.ExceptionColor))
            {
                Exception.WriteLine(FormatLog("EXCEPTION", exception.Message));
            }
        }
        
        public void LogCritical(string message)
        {
            using (new ColorPreset(Palette.CriticalColor))
            {
                Critical.WriteLine(FormatLog("CRITICAL", message));
            }
        }
        
        public void LogCritical(IFormattable message)
        {
            using (new ColorPreset(Palette.CriticalColor))
            {
                Critical.WriteLine(FormatLog("CRITICAL", message));
            }
        }
        
        public void LogCrash(string message)
        {
            using (new ColorPreset(Palette.CrashColor))
            {
                Crash.WriteLine(FormatLog("CRASH", message));
            }
        }
        
        public void LogCrash(IFormattable message)
        {
            using (new ColorPreset(Palette.CrashColor))
            {
                Crash.WriteLine(FormatLog("CRASH", message));
            }
        }
        
        public void LogCustom(string tag, string message)
        {
            using (new ColorPreset(Palette.DefaultColor))
            {
                Custom.WriteLine(FormatLog(tag.ToUpper(), message));
            }
        }

        public void LogCustom(string tag, string message, ConsoleColor color)
        {
            using (new ColorPreset(color))
            {
                Custom.WriteLine(FormatLog(tag.ToUpper(), message));
            }
        }
        
        public void LogCustom(string tag, IFormattable message)
        {
            using (new ColorPreset(Palette.DefaultColor))
            {
                Custom.WriteLine(FormatLog(tag.ToUpper(), message));
            }
        }
        
        public void LogCustom(string tag, IFormattable message, ConsoleColor color)
        {
            using (new ColorPreset(color))
            {
                Custom.WriteLine(FormatLog(tag.ToUpper(), message));
            }
        }

        public void SubscribeLogger(Logger logger)
        {
            // Logging
            logger.OnLog += Log;
            logger.OnLogF += Log;
            logger.OnLogD += Log;
            
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

        private FileStream FileHandle;
        private StreamWriter StreamWriter;
        
        public void Redirect(Debug.LogType logType, TextWriter writer)
        {
            switch (logType)
            {
                case Debug.LogType.Log:
                    Output = writer;
                    break;
                case Debug.LogType.Info:
                    Info = writer;
                    break;
                case Debug.LogType.Warning:
                    Warning = writer;
                    break;
                case Debug.LogType.Error:
                    Error = writer;
                    break;
                case Debug.LogType.Exception:
                    Exception = writer;
                    break;
                case Debug.LogType.Critical:
                    Critical = writer;
                    break;
                case Debug.LogType.Crash:
                    Crash = writer;
                    break;
                case Debug.LogType.Custom:
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
        
        public void Write(Debug.LogType logType, string message, string tag = "")
        {
            switch (logType)
            {
                case Debug.LogType.Log:
                    Output.WriteLine(message);
                    break;
                case Debug.LogType.Info:
                    Info.WriteLine(FormatLog("INFO", message));
                    break;
                case Debug.LogType.Warning:
                    Warning.WriteLine(FormatLog("WARNING", message));
                    break;
                case Debug.LogType.Error:
                    Error.WriteLine(FormatLog("ERROR", message));
                    break;
                case Debug.LogType.Exception:
                    Exception.WriteLine(FormatLog("EXCEPTION", message));
                    break;
                case Debug.LogType.Critical:
                    Critical.WriteLine(FormatLog("CRITICAL", message));
                    break;
                case Debug.LogType.Crash:
                    Crash.WriteLine(FormatLog("CRASH", message));
                    break;
                case Debug.LogType.Custom:
                    Custom.WriteLine(FormatLog(tag, message));
                    break;
            }
        }

        public void Write(Debug.LogType logType, IFormattable message, string tag = "")
        {
            Write(logType, message.ToString(), tag);
        }
    }
}