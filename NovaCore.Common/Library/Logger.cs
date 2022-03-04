using System;
using System.Linq;
using NovaCore.Extensions;

namespace NovaCore.Common
{
    public class Logger : IDisposable
    {
        public event Action OnLoggerClose = delegate {  };

        public delegate void LogEvent(string message);
        
        public delegate void LogEventColored(string message, ConsoleColor color);
        
        public delegate void LogFormattableEvent(IFormattable message);
        
        public delegate void LogObjectEvent(object obj);
        
        public delegate void LogExceptionEvent(Exception exception);
        
        public delegate void LogCustomEvent(string tag, string message);
        
        public delegate void LogCustomEventColored(string tag, string message, ConsoleColor color);
        
        public delegate void LogCustomEventFormattable(string tag, IFormattable message);
        
        public delegate void LogCustomEventFormattableColored(string tag, IFormattable message, ConsoleColor color);
        
        // Log Events

        public event LogEvent OnLog = delegate { };
        
        public event LogEventColored OnLogC = delegate { };
        
        public event LogFormattableEvent OnLogF = delegate { };
        
        public event LogObjectEvent OnLogD = delegate { };

        public void LineBreak()
        {
            OnLog.Invoke("\n");
        }

        public void Log(string message)
        {
            OnLog.Invoke(message);
        }
        
        public void Log(string message, ConsoleColor color)
        {
            OnLogC.Invoke(message, color);
        }
        
        public void Log(IFormattable message)
        {
            OnLogF.Invoke(message);
        }
        
        public void Log(object obj)
        {
            OnLogD.Invoke(obj);
        }
        
        public void Log(params string[] messages)
        {
            OnLog.Invoke(messages.Merge("\n"));
        }

        public void Log(params IFormattable[] messages)
        {
            OnLog.Invoke(messages.Select(s => s.ToString()).Merge("\n"));
        }
        
        // Log Info Events

        public event LogEvent OnLogInfo = delegate { };
        
        public event LogFormattableEvent OnLogInfoF = delegate { };
        
        public void LogInfo(string message)
        {
            OnLogInfo.Invoke(message);
        }
        
        public void LogInfo(IFormattable message)
        {
            OnLogInfoF.Invoke(message);
        }
        
        public void LogInfo(params string[] messages)
        {
            OnLogInfo.Invoke(messages.Merge("\n"));
        }

        public void LogInfo(params IFormattable[] messages)
        {
            OnLogInfo.Invoke(messages.Select(s => s.ToString()).Merge("\n"));
        }
        
        // Log Warning Events
        
        public event LogEvent OnLogWarning = delegate { };
        
        public event LogFormattableEvent OnLogWarningF = delegate { };
        
        public void LogWarning(string message)
        {
            OnLogWarning.Invoke(message);
        }
        
        public void LogWarning(IFormattable message)
        {
            OnLogWarningF.Invoke(message);
        }
        
        public void LogWarning(params string[] messages)
        {
            OnLogWarning.Invoke(messages.Merge("\n"));
        }

        public void LogWarning(params IFormattable[] messages)
        {
            OnLogWarning.Invoke(messages.Select(s => s.ToString()).Merge("\n"));
        }
        
        // Log Error Events
        
        public event LogEvent OnLogError = delegate { };
        
        public event LogFormattableEvent OnLogErrorF = delegate { };
        
        public void LogError(string message)
        {
            OnLogError.Invoke(message);
        }
        
        public void LogError(IFormattable message)
        {
            OnLogErrorF.Invoke(message);
        }
        
        public void LogError(params string[] messages)
        {
            OnLogError.Invoke(messages.Merge("\n"));
        }

        public void LogError(params IFormattable[] messages)
        {
            OnLogError.Invoke(messages.Select(s => s.ToString()).Merge("\n"));
        }
        
        // Log Exception Events
        
        public event LogEvent OnLogException = delegate { };
        
        public event LogFormattableEvent OnLogExceptionF = delegate { };
        
        public event LogExceptionEvent OnLogExceptionE = delegate { };
        
        public void LogException(string message)
        {
            OnLogException.Invoke(message);
        }
        
        public void LogException(IFormattable message)
        {
            OnLogExceptionF.Invoke(message);
        }
        
        public void LogException(Exception exception)
        {
            OnLogExceptionE.Invoke(exception);
        }
        
        public void LogException(params string[] messages)
        {
            OnLogException.Invoke(messages.Merge("\n"));
        }

        public void LogException(params IFormattable[] messages)
        {
            OnLogException.Invoke(messages.Select(s => s.ToString()).Merge("\n"));
        }
        
        // Log Critical Events
        
        public event LogEvent OnLogCritical = delegate { };
        
        public event LogFormattableEvent OnLogCriticalF = delegate { };
        
        public void LogCritical(string message)
        {
            OnLogCritical.Invoke(message);
        }
        
        public void LogCritical(IFormattable message)
        {
            OnLogCriticalF.Invoke(message);
        }
        
        public void LogCritical(params string[] messages)
        {
            OnLogCritical.Invoke(messages.Merge("\n"));
        }

        public void LogCritical(params IFormattable[] messages)
        {
            OnLogCritical.Invoke(messages.Select(s => s.ToString()).Merge("\n"));
        }
        
        // Log Crash Events
        
        public event LogEvent OnLogCrash = delegate { };
        
        public event LogFormattableEvent OnLogCrashF = delegate { };
        
        public void LogCrash(string message)
        {
            OnLogCrash.Invoke(message);
        }
        
        public void LogCrash(IFormattable message)
        {
            OnLogCrashF.Invoke(message);
        }
        
        public void LogCrash(params string[] messages)
        {
            OnLogCrash.Invoke(messages.Merge("\n"));
        }

        public void LogCrash(params IFormattable[] messages)
        {
            OnLogCrash.Invoke(messages.Select(s => s.ToString()).Merge("\n"));
        }
        
        // Log Custom Events
        
        public event LogCustomEvent OnLogCustom = delegate { };
        
        public event LogCustomEventColored OnLogCustomC = delegate { };

        public event LogCustomEventFormattable OnLogCustomF = delegate { };
            
        public event LogCustomEventFormattableColored OnLogCustomFC = delegate { };

        public void LogCustom(string tag, string message)
        {
            OnLogCustom.Invoke(tag, message);
        }
        
        public void LogCustom(string tag, string message, ConsoleColor color)
        {
            OnLogCustomC.Invoke(tag, message, color);
        }
        
        public void LogCustom(string tag, IFormattable message)
        {
            OnLogCustomF.Invoke(tag, message);
        }
        
        public void LogCustom(string tag, IFormattable message, ConsoleColor color)
        {
            OnLogCustomFC.Invoke(tag, message, color);
        }
        
        public void LogCustom(string tag, params string[] messages)
        {
            OnLogCustom.Invoke(tag, messages.Merge("\n"));
        }

        public void LogCustom(string tag, params IFormattable[] messages)
        {
            OnLogCustom.Invoke(tag, messages.Select(s => s.ToString()).Merge("\n"));
        }

        public void Dispose()
        {
            OnLoggerClose.Invoke();
        }
    }
}