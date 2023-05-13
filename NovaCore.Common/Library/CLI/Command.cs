using System.Collections.Generic;
using System.Linq;

namespace NovaCore.Common.CLI;

// TODO: Flags: --word / -w / -word
public readonly struct Command
{
    public delegate void CommandEvent(string cmd, int argc, string[] argv);

    public static CommandEvent OnCommandBroadcast = delegate {  };
        
    public readonly string Cmd;
    public readonly int Argc;
    public readonly string[] Argv;

    public Command(IReadOnlyList<string> tokens)
    {
        Cmd = tokens[0];
        Argc = tokens.Count - 1;
        Argv = Argc == 0 ? null : tokens.Skip(1).ToArray();
    }

    public void Invoke()
    {
        OnCommandBroadcast.Invoke(Cmd, Argc, Argv);
    }
}