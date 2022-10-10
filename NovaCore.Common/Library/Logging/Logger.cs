using System;
using System.Linq;
using NovaCore.Common.Extensions;

namespace NovaCore.Common.Logging
{
    public partial class Logger : IDisposable
    {
        private readonly string _moduleName;
        
        public Logger()
        {
            
        }

        public Logger(string moduleName)
        {
            _moduleName = moduleName;
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
        
        // LogText
        
        public void LogText(string message)
        {
            OnLogText.Invoke(message);
        }
        
        public void LogText(string message, ConsoleColor color)
        {
            OnLogTextC.Invoke(message, color);
        }
        
        public void LogText(IFormattable message)
        {
            OnLogTextF.Invoke(message);
        }
        
        public void LogText(object obj)
        {
            OnLogTextD.Invoke(obj);
        }
        
        // LogInfo
        
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
        
        // Log Warning
        
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
        
        // Log Error
        
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
        
        // Log Exception
        
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
        
        // Log Critical
        
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
        
        // Log Crash
        
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
        
        // Log Custom

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