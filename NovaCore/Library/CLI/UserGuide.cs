using System;
using System.Collections.Generic;
using NovaCore.CLI.Interpreters;
using NovaCore.Core;
using NovaCore.Extensions;
using Debug = NovaCore.Logging.Debug;

namespace NovaCore.CLI
{
    public static class UserGuide
    {
        private static readonly Dictionary<Command, string> HelpDictionary = new Dictionary<Command, string>
        {
            { Command.SAVE,     "SAVE - " },
            { Command.LOAD,     "LOAD - " },
            { Command.MERGE,    "MERGE - " },
            { Command.EXIT,     "EXIT - " },
            { Command.HELP,     "HELP - Displays user help messages" },
            { Command.OPEN,     "OPEN - " },
            { Command.CLOSE,    "CLOSE - " },
            { Command.INSERT,   "INSERT - " },
            { Command.DELETE,   "DELETE - " },
            { Command.SWAP,     "SWAP - " },
            { Command.PUSH,     "PUSH - " },
            { Command.PULL,     "PULL - " },
            { Command.SYNC,     "SYNC - " },
            { Command.UPDATE,   "UPDATE - " }
        };

        // TODO: Mode specific (mode parameter) help, use flags to check on another MediaSpace
        public static void Help(MediaSpace.Mode mode, string command)
        {
            if (Interpreter.TryParse(command, out Command cmd))
            {
                HelpLog(HelpDictionary[cmd]);
            }
            else
            {
                Interpreter.Usage("\"HELP {command}\"");
            }
        }

        private static void HelpLog(string message)
        {
            Debug.LogCustom("HELP", message, ConsoleColor.Magenta);
        }

        public static void General()
        {
            HelpLog(GENERAL_MESSAGE);
        }

        // Does not display current platform properly
        private static readonly string GENERAL_MESSAGE = 
            $@"For help on a specific platform or command, type ""HELP"" followed by the platform or command.

Platforms: {Flags.GetValues<MediaSpace.Mode>().MergeWrap("{", "}")}

Commands (global): {Interpreter.GetGlobalCommands().MergeWrap("{", "}")}

Commands (current platform): {Program.Data.InterpreterCollection.GetCurrentCommands().MergeWrap("{", "}")}

Commands (all): {Flags.GetValues<Command>().MergeWrap("{", "}")}";
    }
}