using System;
using System.Linq;
using System.Text;
using NovaCore.Common.Extensions;

namespace NovaCore.Common.Logging
{
    public partial class Logger : IDisposable
    {
        private readonly StringBuilder _log = new();
        private readonly string _moduleName;
        private readonly string _subModuleName;
        
        public Logger()
        {
            
        }

        public Logger(string moduleName)
        {
            _moduleName = moduleName;
        }

        public string GetSerialized()
        {
            return _log.ToString();
        }

        public override string ToString()
        {
            return _log.ToString();
        }

        public void Log(string message)
        {
            _log.AppendLine(message);
            OnLog.Invoke(message);
        }
        
        public void Log(string message, ConsoleColor color)
        {
            _log.AppendLine(message);
            OnLogC.Invoke(message, color);
        }
        
        public void Log(IFormattable message)
        {
            _log.AppendLine(message.ToString());
            OnLogF.Invoke(message);
        }
        
        public void Log(object obj)
        {
            _log.AppendLine(obj.ToString());
            OnLogD.Invoke(obj);
        }
        
        public void Log(params string[] messages)
        {
            string message = messages.Merge("\n");
            _log.AppendLine(message);
            OnLog.Invoke(message);
        }

        public void Log(params IFormattable[] messages)
        {
            string message = messages.Select(s => s.ToString()).Merge("\n");
            _log.AppendLine(message);
            OnLog.Invoke(message);
        }
        
        // LogText
        
        public void LogText(string message)
        {
            _log.Append(message);
            OnLogText.Invoke(message);
        }
        
        public void LogText(string message, ConsoleColor color)
        {
            _log.Append(message);
            OnLogTextC.Invoke(message, color);
        }
        
        public void LogText(IFormattable message)
        {
            _log.Append(message);
            OnLogTextF.Invoke(message);
        }
        
        public void LogText(object obj)
        {
            _log.Append(obj);
            OnLogTextD.Invoke(obj);
        }
        
        // LogInfo
        
        public void LogInfo(string message)
        {
            _log.Append($"[INFO] {message}");
            OnLogInfo.Invoke(message);
        }
        
        public void LogInfo(IFormattable message)
        {
            _log.Append($"[INFO] {message}");
            OnLogInfoF.Invoke(message);
        }
        
        public void LogInfo(params string[] messages)
        {
            string message = messages.Merge("\n");
            _log.Append($"[INFO] {message}");
            OnLogInfo.Invoke(message);
        }

        public void LogInfo(params IFormattable[] messages)
        {
            string message = messages.Select(s => s.ToString()).Merge("\n");
            _log.Append($"[INFO] {message}");
            OnLogInfo.Invoke(message);
        }
        
        // Log Warning
        
        public void LogWarning(string message)
        {
            _log.Append($"[WARNING] {message}");
            OnLogWarning.Invoke(message);
        }
        
        public void LogWarning(IFormattable message)
        {
            _log.Append($"[WARNING] {message}");
            OnLogWarningF.Invoke(message);
        }
        
        public void LogWarning(params string[] messages)
        {
            string message = messages.Merge("\n");
            _log.Append($"[WARNING] {message}");
            OnLogWarning.Invoke(message);
        }

        public void LogWarning(params IFormattable[] messages)
        {
            string message = messages.Select(s => s.ToString()).Merge("\n");
            _log.Append($"[WARNING] {message}");
            OnLogWarning.Invoke(message);
        }
        
        // Log Error
        
        public void LogError(string message)
        {
            _log.Append($"[EXCEPTION] {message}");
            OnLogError.Invoke(message);
        }
        
        public void LogError(IFormattable message)
        {
            _log.Append($"[EXCEPTION] {message}");
            OnLogErrorF.Invoke(message);
        }
        
        public void LogError(params string[] messages)
        {
            string message = messages.Merge("\n");
            _log.Append($"[EXCEPTION] {message}");
            OnLogError.Invoke(message);
        }

        public void LogError(params IFormattable[] messages)
        {
            string message = messages.Select(s => s.ToString()).Merge("\n");
            _log.Append($"[EXCEPTION] {message}");
            OnLogError.Invoke(message);
        }
        
        // Log Exception
        
        public void LogException(string message)
        {
            _log.Append($"[EXCEPTION] {message}");
            OnLogException.Invoke(message);
        }
        
        public void LogException(IFormattable message)
        {
            _log.Append($"[EXCEPTION] {message}");
            OnLogExceptionF.Invoke(message);
        }
        
        public void LogException(Exception exception)
        {
            _log.Append($"[EXCEPTION] {exception}");
            OnLogExceptionE.Invoke(exception);
        }
        
        public void LogException(params string[] messages)
        {
            string message = messages.Merge("\n");
            _log.Append($"[EXCEPTION] {message}");
            OnLogException.Invoke(message);
        }

        public void LogException(params IFormattable[] messages)
        {
            string message = messages.Select(s => s.ToString()).Merge("\n");
            _log.Append($"[EXCEPTION] {message}");
            OnLogException.Invoke(message);
        }
        
        // Log Critical
        
        public void LogCritical(string message)
        {
            _log.Append($"[CRITICAL] {message}");
            OnLogCritical.Invoke(message);
        }
        
        public void LogCritical(IFormattable message)
        {
            _log.Append($"[CRITICAL] {message}");
            OnLogCriticalF.Invoke(message);
        }
        
        public void LogCritical(params string[] messages)
        {
            string message = messages.Merge("\n");
            _log.Append($"[CRITICAL] {message}");
            OnLogCritical.Invoke(message);
        }

        public void LogCritical(params IFormattable[] messages)
        {
            string message = messages.Select(s => s.ToString()).Merge("\n");
            _log.Append($"[CRITICAL] {message}");
            OnLogCritical.Invoke(message);
        }
        
        // Log Crash
        
        public void LogCrash(string message)
        {
            _log.Append($"[CRASH] {message}");
            OnLogCrash.Invoke(message);
        }
        
        public void LogCrash(IFormattable message)
        {
            _log.Append($"[CRASH] {message}");
            OnLogCrashF.Invoke(message);
        }
        
        public void LogCrash(params string[] messages)
        {
            string message = messages.Merge("\n");
            _log.Append($"[CRASH] {message}");
            OnLogCrash.Invoke(message);
        }

        public void LogCrash(params IFormattable[] messages)
        {
            string message = messages.Select(s => s.ToString()).Merge("\n");
            _log.Append($"[CRASH] {message}");
            OnLogCrash.Invoke(message);
        }
        
        // Log Custom

        public void LogCustom(string tag, string message)
        {
            _log.Append($"[{tag.ToUpper()}] {message}");
            OnLogCustom.Invoke(tag, message);
        }
        
        public void LogCustom(string tag, string message, ConsoleColor color)
        {
            _log.Append($"[{tag.ToUpper()}] {message}");
            OnLogCustomC.Invoke(tag, message, color);
        }
        
        public void LogCustom(string tag, IFormattable message)
        {
            _log.Append($"[{tag.ToUpper()}] {message}");
            OnLogCustomF.Invoke(tag, message);
        }
        
        public void LogCustom(string tag, IFormattable message, ConsoleColor color)
        {
            _log.Append($"[{tag.ToUpper()}] {message}");
            OnLogCustomFC.Invoke(tag, message, color);
        }
        
        public void LogCustom(string tag, params string[] messages)
        {
            string message = messages.Merge("\n");
            _log.Append($"[{tag.ToUpper()}] {message}");
            OnLogCustom.Invoke(tag, message);
        }

        public void LogCustom(string tag, params IFormattable[] messages)
        {
            string message = messages.Select(s => s.ToString()).Merge("\n");
            _log.Append($"[{tag.ToUpper()}] {message}");
            OnLogCustom.Invoke(tag, message);
        }

        public void Dispose()
        {
            OnLoggerClose.Invoke();
        }
    }
}