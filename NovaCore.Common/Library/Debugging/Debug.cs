using System.Collections.Generic;
using System.IO;
using System.Linq;
using NovaCore.Common.Extensions;
using NovaCore.Common.Logging;
using NovaCore.Common.Utilities;

namespace NovaCore.Common.Debugging;

public static partial class Debug
{
    // Output Pipelines
    private static LoggingChannel PrimaryChannel = new(Output.StandardOutput);
        
    // Default Channel?

    public enum Mode
    {
        DEFAULT,        //< Logs, Info, Warnings, Errors
        MINIMAL,        //< Logs, Info
        VERBOSE,        //< Exceptions tracked
        STACKTRACE,     //< Exceptions include stacktrace info
        SILENT,         //< Logs only (no file output)
        NO_WARNINGS,    //< Logs, Errors
        NO_ERRORS       //< Logs, Warnings
    }

    public static readonly Dictionary<Mode, LogLevel[]> Includes = new()
    {
        { Mode.MINIMAL,     new [] { LogLevel.Log, LogLevel.Info } },
        { Mode.SILENT,      new [] { LogLevel.Log } },
        { Mode.NO_WARNINGS, new [] { LogLevel.Log, LogLevel.Info, LogLevel.Error } },
        { Mode.NO_ERRORS,   new [] { LogLevel.Log, LogLevel.Info, LogLevel.Warning } },
    };

    public static List<LogLevel> GetExcludedLogTypes(Mode debugMode)
    {
        return Flags.GetValues<LogLevel>().Except(Includes[debugMode]).ToList();
    }

    public static Mode DebugMode { get; private set; } = Mode.DEFAULT;

    public static void SetDebugMode(Mode debugMode)
    {
        DebugMode = debugMode;
        switch (debugMode)
        {
            case Mode.VERBOSE:
                Output.TrackExceptions();
                break;
            case Mode.MINIMAL:
            case Mode.SILENT:
            case Mode.NO_WARNINGS:
            case Mode.NO_ERRORS:
                GetExcludedLogTypes(debugMode).ForEach(Suppress);
                break;
            case Mode.DEFAULT:
            case Mode.STACKTRACE:
            default:
                break;
        }
    }
        
    public static void SubscribeToDefault(Logger logger)
    {
        PrimaryChannel.SubscribeLogger(logger);
    }

    public static void SubscribeToDefault(params Logger[] loggers)
    {
        loggers.ForEach(PrimaryChannel.SubscribeLogger);
    }
        
    public static void UnsubscribeFromDefault(Logger logger)
    {
        PrimaryChannel.UnsubscribeLogger(logger);
    }
        
    public static void UnsubscribeFromDefault(params Logger[] loggers)
    {
        loggers.ForEach(PrimaryChannel.UnsubscribeLogger);
    }
        
    public static void Suppress(LogLevel logLevel)
    {
        PrimaryChannel.Redirect(logLevel, TextWriter.Null);
    }

    public static void SetDebugOutput<T>(T redirectedOutput = null) where T : TextWriter
    {
        PrimaryChannel.Output = redirectedOutput ?? Output.StandardOutput;
    }
}