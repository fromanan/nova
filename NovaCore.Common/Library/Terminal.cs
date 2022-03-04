using System;
using System.Linq;
using System.Text.RegularExpressions;
using NovaCore.Common;

namespace NovaCore.CLI
{
    public class Terminal : IDisposable
    {
        public readonly Logger Logger = new();
        
        public delegate void CommandEvent(string cmd, int argc, string[] argv);

        public CommandEvent OnCommandBroadcast = delegate {  };

        public bool ReceiveInput { get; private set; } = true;
        
        public int LineCount { get; private set; }
        
        public static ConsoleColor InputColor = ConsoleColor.Green;

        public Terminal()
        {
            Init();
        }
        
        public Terminal(Logger logger)
        {
            Logger = logger;
            Init();
        }
        
        public void Dispose() { }

        private void Init()
        {
            NovaApp.ShutdownEvent += Close;
        }

        public void Run()
        {
            while (ReceiveInput) GetLine();
        }

        public void Close()
        {
            ReceiveInput = false;
        }

        private void GetLine()
        {
            Parse(Prompt("Please enter a command"));
        }
        
        public string Prompt(string message, bool useCount = true)
        {
            Logger.Log(useCount ? $"< {LineCount++} > [{message}]" : $"{message}:");
            string input = Console.ReadLine();
            Logger.LineBreak();
            return input;
        }

        public static string Input(string message = "> ")
        {
            Console.WriteLine(message);
            Console.ForegroundColor = InputColor;
            string input = Console.ReadLine();
            Console.ResetColor();
            return input;
        }

        public bool Confirm(string action)
        {
            Logger.Log($"Are you sure you want to {action}? [Y/N]");
            return Input().ToUpper() is "Y" or "YE" or "YES";
        }
        
        private static string Sanitize(string input)
        {
            return Regex.Replace(input, @"[^\w\.@-_""''\s]", string.Empty);
        }

        // https://stackoverflow.com/questions/4780728/regex-split-string-preserving-quotes/4780801#4780801
        // https://stackoverflow.com/questions/14655023/split-a-string-that-has-white-spaces-unless-they-are-enclosed-within-quotes
        private static string[] Tokenize(string input)
        {
            return Regex.Split(input, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            /*return input.Split('"')
                .Select((element, index) => index % 2 == 0  // If even index
                    ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)  // Split the item
                    : new [] { element })  // Keep the entire item
                .SelectMany(element => element).ToArray();*/
        }

        private void Parse(string input)
        {
            // Ignore a blank input
            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }
            
            // Remove all invalid characters
            string sanitizedInput = Sanitize(input);
            if (string.IsNullOrWhiteSpace(sanitizedInput))
            {
                Logger.LogError("Failed to Parse Command");
                return;
            }
            
            // Break the input into tokens (command, arguments, ect.)
            string[] tokens = Tokenize(sanitizedInput);
            if (tokens == null)
            {
                Logger.LogError("Failed to Tokenize Command");
                return;
            }

            // Formatting input into the Unix CLI Format
            string cmd = tokens[0];
            int argc = tokens.Length - 1;
            string[] argv = argc == 0 ? null : tokens.Skip(1).ToArray();

            // Broadcast CLI input
            OnCommandBroadcast.Invoke(cmd, argc, argv);
        }
        
        // TODO: Command Class
        // Flags: -- / -
        // Arguments
        // Count
    }
}