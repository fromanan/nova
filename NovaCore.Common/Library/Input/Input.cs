using System;
using System.IO;
using NovaCore.Common.Logging;
using Logger = NovaCore.Common.Logging.Logger;

namespace NovaCore.Common
{
    public static partial class Input
    {
        public static TextReader StandardInput { get; private set; } = Console.In;
        
        public static readonly Logger Logger = new();
        
        private static int _count; //< Number of commands executed
        
        public static ConsoleColor Color { get; private set; } = ConsoleColor.Green;

        public static bool UseStandardOut;

        public static void Init(LoggingChannel loggingChannel = null)
        {
            if (loggingChannel is not null)
            {
                loggingChannel.SubscribeLogger(Logger);
            }
            else
            {
                UseStandardOut = true;
            }
        }

        public static void SetInputColor(ConsoleColor color)
        {
            Color = color;
        }

        public static string Get()
        {
            ConsoleColor oldColor = Output.SetTextColor(Color);
            string input = StandardInput.ReadLine();
            Output.SetTextColor(oldColor);
            Output.LineBreak();
            return input;
        }
        
        public static string GetWithPrompt(string prompt, string message = "> ")
        {
            if (UseStandardOut)
            {
                Output.WriteLine(prompt);
                Output.Write(message);
            }
            else
            {
                Logger.Log(prompt);
                Logger.LogText(message);
            }
            
            return Get();
        }

        public static string GetWithMessage(string message = "> ")
        {
            if (UseStandardOut)
            {
                Output.Write(message);
            }
            else
            {
                Logger.LogText(message);
            }
            return Get();
        }

        public static string Prompt(string message, bool useCount = true)
        {
            return GetWithPrompt(useCount ? $"< {_count++} > [{message}]" : $"{message}:");
        }

        public static bool Confirm(string action)
        {
            return GetWithPrompt($"Are you sure you want to {action}? [Y/N]").ToUpper() is "Y" or "YE" or "YES";
        }
        
        public static void ShowCursor() => Console.CursorVisible = true;
        
        public static void HideCursor() => Console.CursorVisible = false;
    }
}