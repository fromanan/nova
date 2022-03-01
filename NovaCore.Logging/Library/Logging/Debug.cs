using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NovaCore.Files;
using NovaCore.Utilities;

namespace NovaCore.Logging
{
    public static partial class Debug
    {
        // Standard Streams
        public static TextReader StandardInput { get; private set; } = Console.In;
        
        public static TextWriter StandardOutput { get; private set; } = Console.Out;
        
        public static TextWriter StandardError { get; private set; } = Console.Error;
        
        // Output Pipelines
        private static LoggingChannel PrimaryChannel = new(StandardOutput);
        
        // Class Properties
        private static int count; //< Number of commands executed
        private static bool redirected;

        // Exception Stack
        private static readonly Stack<Exception> ExceptionStack = new();
        
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

        public static readonly Dictionary<Mode, LogType[]> Includes = new()
        {
            { Mode.MINIMAL,     new [] { LogType.Log, LogType.Info } },
            { Mode.SILENT,      new [] { LogType.Log } },
            { Mode.NO_WARNINGS, new [] { LogType.Log, LogType.Info, LogType.Error } },
            { Mode.NO_ERRORS,   new [] { LogType.Log, LogType.Info, LogType.Warning } },
        };

        public static List<LogType> GetExcludedLogTypes(Mode debugMode)
        {
            return Flags.GetValues<LogType>().Except(Includes[debugMode]).ToList();
        }

        public static Mode DebugMode { get; private set; } = Mode.DEFAULT;

        public static void SetDebugMode(Mode debugMode)
        {
            DebugMode = debugMode;
            switch (debugMode)
            {
                case Mode.VERBOSE:
                    TrackExceptions();
                    break;
                case Mode.MINIMAL:
                case Mode.SILENT:
                case Mode.NO_WARNINGS:
                case Mode.NO_ERRORS:
                    GetExcludedLogTypes(debugMode).ForEach(Suppress);
                    break;
            }
        }

        public static void Init()
        {
            Windowing.CreateConsole();
            StandardOutput = Console.Out;
        }
        
        public static void Suppress(LogType logType)
        {
            PrimaryChannel.Redirect(logType, TextWriter.Null);
        }

        public static void Redirect(TextWriter redirectedOutput = null)
        {
            Redirect<TextWriter>(redirectedOutput);
        }

        public static void Redirect<T>(T redirectedOutput = null) where T : TextWriter
        {
            // Reset the Output Stream
            if (redirectedOutput == null)
            {
                Console.SetOut(StandardOutput);
                redirected = false;
            }
            // Set the Output Stream to the TextWriter specified
            else if (redirected)
            {
                LogWarning("Cannot redirect output twice");
            }
            else
            {
                Console.SetOut(redirectedOutput);
                redirected = true;
            }
        }

        public static T OpenBuffer<T>(bool redirect = true) where T : TextWriter, new()
        {
            T buffer = new T();
            
            if (redirect)
            {
                Redirect(buffer);
            }

            return buffer;
        }

        public static void CloseBuffer<T>(T buffer) where T : TextWriter
        {
            buffer.Close();
            Redirect();
        }

        public static void SetDebugOutput<T>(T redirectedOutput = null) where T : TextWriter
        {
            PrimaryChannel.Output = redirectedOutput ?? StandardOutput;
        }

        public static void ShowCursor() => Console.CursorVisible = true;
        public static void HideCursor() => Console.CursorVisible = false;

        private static StreamWriter ExceptionWriter;
        
        private static string ExceptionHeader =>
            $"[ Exception {FileSystem.Guid()} - {DateTime.Now:MM/dd/yyyy hh:mm:ss.fff tt} ]";

        private static string FileHeader()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine($"{AppInfo.ProductName} Version: {AppInfo.ProductVersion}");
            buffer.AppendLine($"Application started at: {DateTime.Now:G}");
            return buffer.ToString();
        }

        public static void TrackExceptions()
        {
            // Ensure Log Folder Exists
            FileSystem.CreateFolder(Application.Paths.Log);

            // Create Exception Log in Files
            string filename = $"{FileSystem.TimestampFilename("exception")}.log";
            string filepath = Path.Combine(Application.Paths.Log, filename);
            ExceptionWriter = File.AppendText(filepath);
            
            // Write the Header
            ExceptionWriter.WriteLine(FileHeader());
            
            // Track First Chance Exceptions
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                if (e?.Exception == null) return;
                ExceptionStack.Push(e.Exception);
                ExceptionWriter.WriteLine(ExceptionHeader);
                ExceptionWriter.WriteLine(e.Exception);
                ExceptionWriter.WriteLine();
            };

            // Track Unhandled Exceptions
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (e?.ExceptionObject == null) return;
                ExceptionStack.Push(e.ExceptionObject as Exception);
                ExceptionWriter.WriteLine(ExceptionHeader);
                ExceptionWriter.WriteLine(e.ExceptionObject);    
                ExceptionWriter.WriteLine();
            };
        }
    }
}