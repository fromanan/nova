using System;
using System.Linq;
using System.Text.RegularExpressions;
using Debug = NovaCore.Logging.Debug;

namespace NovaCore.CLI
{
    public class Terminal : IDisposable
    {
        public delegate void CommandEvent(string cmd, int argc, string[] argv);

        public static CommandEvent OnCommandBroadcast = delegate {  };
        
        public static Action OnInstanceClose;

        public bool ReceiveInput { get; private set; } = true;

        public Terminal()
        {
            Initialize();
        }
        
        public void Dispose() { }

        private void Initialize()
        {
            OnInstanceClose += CloseInstance;
        }

        public void Run()
        {
            while (ReceiveInput) Input();
        }

        private void CloseInstance()
        {
            ReceiveInput = false;
        }

        private static void Input()
        {
            Parse(Debug.Prompt("Please enter a command"));
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

        private static void Parse(string input)
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
                Debug.LogError("Failed to Parse Command");
                return;
            }
            
            // Break the input into tokens (command, arguments, ect.)
            string[] tokens = Tokenize(sanitizedInput);
            if (tokens == null)
            {
                Debug.LogError("Failed to Tokenize Command");
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