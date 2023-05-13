using System;
using System.Collections.Generic;
using NovaCore.Common.Interfaces;
using ParserResult = NovaCore.Common.CLI.CommandParser.ParserResult;

namespace NovaCore.Common.CLI;

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
        Parse(Input.Prompt(ConsoleMessages.Prompt));
    }

    private void Parse(string input)
    {
        switch (CommandParser.Parse(input, out Command? command))
        {
            case ParserResult.Empty:
                return;
            case ParserResult.SanitizeFailed:
                Logger.LogError(ConsoleMessages.FailedSanitize);
                return;
            case ParserResult.TokenizeFailed:
                Logger.LogError(ConsoleMessages.FailedTokenize);
                return;
            case ParserResult.Success:
                // Broadcast CLI input
                if (command is not { } cmd)
                    return;
                _commandQueue.Enqueue(cmd);
                cmd.Invoke();
                return;
            default:
                throw new NotImplementedException();
        }
    }
}