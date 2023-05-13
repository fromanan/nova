using System;

namespace NovaCore.Common.Logging;

public partial class Logger
{
    public event Action OnLoggerClose = delegate {  };

    // Log Events
        
    public delegate void LogEvent(string message);

    public delegate void LogEventColored(string message, ConsoleColor color);
        
    public delegate void LogFormattableEvent(IFormattable message);
        
    public delegate void LogObjectEvent(object obj);

    public event LogEvent OnLog = delegate { };
        
    public event LogEventColored OnLogC = delegate { };
        
    public event LogFormattableEvent OnLogF = delegate { };
        
    public event LogObjectEvent OnLogD = delegate { };
        
    // Log Text Events
        
    public delegate void LogTextEvent(string message);

    public delegate void LogTextEventColored(string message, ConsoleColor color);
        
    public delegate void LogTextFormattableEvent(IFormattable message);
        
    public delegate void LogTextObjectEvent(object obj);

    public event LogTextEvent OnLogText = delegate { };
        
    public event LogTextEventColored OnLogTextC = delegate { };
        
    public event LogTextFormattableEvent OnLogTextF = delegate { };
        
    public event LogTextObjectEvent OnLogTextD = delegate { };
        
    // Log Info Events

    public event LogEvent OnLogInfo = delegate { };
        
    public event LogFormattableEvent OnLogInfoF = delegate { };
        
    // Log Warning Events
        
    public event LogEvent OnLogWarning = delegate { };
        
    public event LogFormattableEvent OnLogWarningF = delegate { };
        
    // Log Error Events
        
    public event LogEvent OnLogError = delegate { };
        
    public event LogFormattableEvent OnLogErrorF = delegate { };
        
    // Log Exception Events
        
    public delegate void LogExceptionEvent(Exception exception);
        
    public event LogEvent OnLogException = delegate { };
        
    public event LogFormattableEvent OnLogExceptionF = delegate { };
        
    public event LogExceptionEvent OnLogExceptionE = delegate { };
        
    // Log Critical Events
        
    public event LogEvent OnLogCritical = delegate { };
        
    public event LogFormattableEvent OnLogCriticalF = delegate { };
        
    // Log Crash Events
        
    public event LogEvent OnLogCrash = delegate { };
        
    public event LogFormattableEvent OnLogCrashF = delegate { };
        
    // Log Custom Events
        
    public delegate void LogCustomEvent(string tag, string message);
        
    public delegate void LogCustomEventColored(string tag, string message, ConsoleColor color);
        
    public delegate void LogCustomEventFormattable(string tag, IFormattable message);
        
    public delegate void LogCustomEventFormattableColored(string tag, IFormattable message, ConsoleColor color);
        
    public event LogCustomEvent OnLogCustom = delegate { };
        
    public event LogCustomEventColored OnLogCustomC = delegate { };

    public event LogCustomEventFormattable OnLogCustomF = delegate { };
            
    public event LogCustomEventFormattableColored OnLogCustomFC = delegate { };
}