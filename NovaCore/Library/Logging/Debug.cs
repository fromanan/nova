using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using NovaCore.Library.Files;

namespace NovaCore.Library.Logging
{
    public static partial class Debug
    {
        public static readonly TextReader StandardInput = Console.In;
        public static readonly TextWriter StandardOutput = Console.Out;
        public static readonly TextWriter StandardError = Console.Error;
        private static TextWriter Output = StandardOutput;
        private static TextWriter RedirectedOutput = TextWriter.Null;
        private static int count; //< Number of commands executed
        private static bool redirected;

        private static readonly Stack<Exception> ExceptionStack = new Stack<Exception>();
        
        public enum Mode
        {
            DEFAULT,        //< Logs, Info, Warnings, Errors
            MINIMAL,        //< Logs only
            VERBOSE,        //< Exceptions tracked
            STACKTRACE,     //< Exceptions include stacktrace info
            SILENT          //< No logging
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
                case Mode.SILENT:
                    SetDebugOutput(TextWriter.Null);
                    break;
            }
        }

        public static double Elapsed(DateTime startTime)
        {
            return (DateTime.Now - startTime).TotalMilliseconds;
        }
        
        public static double ElapsedSeconds(DateTime startTime)
        {
            return (DateTime.Now - startTime).TotalSeconds;
        }
        
        public static void Timestamp(DateTime startTime)
        {
            Output.WriteLine($"Time Elapsed: {ElapsedSeconds(startTime):F2} seconds");
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
                RedirectedOutput.Dispose();
                RedirectedOutput = TextWriter.Null;
                redirected = false;
            }
            // Set the Output Stream to the TextWriter specified
            else if (redirected)
            {
                LogWarning("Cannot redirect output twice");
            }
            else
            {
                RedirectedOutput = redirectedOutput;
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

        public static string Prompt(string message, bool useCount = true)
        {
            Output.WriteLine(useCount ? $"< {count++} > [{message}]" : $"{message}:");
            return Input();
        }

        public static string Input(string message = "> ")
        {
            Output.Write(message);
            string input = StandardInput.ReadLine();
            Output.WriteLine();
            return input;
        }

        public static void LineBreak()
        {
            Output.WriteLine();
        }

        public static bool Confirm(string action)
        {
            Output.WriteLine($"Are you sure you want to {action}? [Y/N]");
            Output.Write("> ");
            string input = StandardInput.ReadLine()?.ToUpper();
            return input == "Y" || input == "YE" || input == "YES";
        }

        private static StreamWriter exceptionWriter;

        private static string ExceptionHeader =>
            $"[ Exception {FileSystem.Guid()} - {DateTime.Now:MM/dd/yyyy hh:mm:ss.fff tt} ]";

        public static void TrackExceptions()
        {
            if (!Directory.Exists(Program.Paths.Log))
            {
                Directory.CreateDirectory(Program.Paths.Log);
            }

            string filename = $"{FileSystem.TimestampFilename("exception")}.log";
            exceptionWriter = File.AppendText(Path.Combine(Program.Paths.Log, filename));
            exceptionWriter.WriteLine($"{Application.ProductName} Version: {Application.ProductVersion}");
            exceptionWriter.WriteLine($"Application started at: {DateTime.Now:G}\n");
            
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                if (e?.Exception == null) return;
                exceptionWriter.WriteLine(ExceptionHeader);
                exceptionWriter.WriteLine(e.Exception);
                exceptionWriter.WriteLine();
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (e?.ExceptionObject == null) return;
                exceptionWriter.WriteLine(ExceptionHeader);
                exceptionWriter.WriteLine(e.ExceptionObject);    
                exceptionWriter.WriteLine();
            };
        }
        
        // https://stackoverflow.com/questions/24918768/progress-bar-in-console-application
        private static void ProgressBar(int progress, int total)
        {
            // Draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); // Start
            Console.CursorLeft = 32;
            Console.Write("]"); // End
            Console.CursorLeft = 1;
            float oneChunk = 30.0f / total;

            // Draw filled part
            int position = 1;
            for (int i = 0; i < oneChunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            // Draw unfilled part
            for (int i = position; i <= 31 ; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            // Draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"{progress} of {total}\t"); // Blanks at the end remove any excess
        }

        public static void Separator(bool lineBreak = false)
        {
            string separator = new string('=', 50);
            Output.WriteLine(lineBreak ? $"{separator}\n" : separator);
        }

        public static void Header(string headerName)
        {
            int headerLength = headerName.Length;
            
            if (headerLength > 42)
            {
                LogWarning($"Header ignored, invalid length of {headerLength} (maximum of 42 characters)");
                return;
            }
            
            // 50 (default line length) - 2 (outside space) - 2 (brackets) - 2 (inside space) => 44
            string side = new string('=', (44 - headerLength)/2);

            Output.WriteLine(headerLength % 2 == 0
                ? $"{side} [ {headerName} ] {side}"
                : $"{side} [ {headerName}  ] {side}");
        }

        public static int LineNumber([CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }

        public static void SetDebugOutput<T>(T redirectedOutput = null) where T : TextWriter
        {
            Output = redirectedOutput ?? StandardOutput;
        }

        public static void Suppress()
        {
            Redirect(TextWriter.Null);
        }
    }
}