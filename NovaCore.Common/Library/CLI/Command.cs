using System.Collections.Generic;
using System.Linq;

namespace NovaCore.Common.CLI
{
    // TODO: Flags: --word / -w / -word
    public readonly struct  Command
    {
        public delegate void CommandEvent(string cmd, int argc, string[] argv);

        public static CommandEvent OnCommandBroadcast = delegate {  };
        
        public readonly string cmd;
        public readonly int argc;
        public readonly string[] argv;

        public Command(IReadOnlyList<string> tokens)
        {
            cmd = tokens[0];
            argc = tokens.Count - 1;
            argv = argc == 0 ? null : tokens.Skip(1).ToArray();
        }

        public void Invoke()
        {
            OnCommandBroadcast.Invoke(cmd, argc, argv);
        }
    }
}