using System;
using System.Collections.Generic;
using ParserResult = NovaCore.Common.CLI.CommandParser.ParserResult;

namespace NovaCore.Common.CLI
{
    public partial class Terminal : IDisposable, INovaShutdown
    {
        public readonly Logging.Logger Logger = new();
        
        public bool ReceiveInput { get; private set; } = true;

        private readonly Queue<Command> _commandQueue = new();

        public Terminal()
        {
            Global.Subscribe(this);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Run()
        {
            while (ReceiveInput) GetInput();
        }

        public void OnShutdown()
        {
            ReceiveInput = false;
        }

        private void GetInput()
        {
            Parse(Input.Prompt(Messages.PROMPT));
        }

        private static class Messages
        {
            public const string PROMPT = "Please enter a command";
            public const string FAILED_SANITIZE = "Failed to Parse Command";
            public const string FAILED_TOKENIZE = "Failed to Tokenize Command";
        }

        private void Parse(string input)
        {
            switch (CommandParser.Parse(input, out Command? command))
            {
                case ParserResult.Empty:
                    return;
                case ParserResult.SanitizeFailed:
                    Logger.LogError(Messages.FAILED_SANITIZE);
                    return;
                case ParserResult.TokenizeFailed:
                    Logger.LogError(Messages.FAILED_TOKENIZE);
                    return;
                case ParserResult.Success:
                    // Broadcast CLI input
                    if (command is not null)
                    {
                        _commandQueue.Enqueue((Command)command);
                    }
                    command?.Invoke();
                    return;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}