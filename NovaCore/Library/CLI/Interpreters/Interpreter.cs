using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using NovaCore.Core;
using Debug = NovaCore.Logging.Debug;

namespace NovaCore.CLI.Interpreters
{
    public abstract class Interpreter
    {
        public delegate void CommandAction(int argc, string[] argv);
        
        public class CommandDictionary : Dictionary<Command, CommandAction> { }

        private static readonly CommandDictionary GlobalCommands = new CommandDictionary
        {
            { Command.EXIT,         Exit },
            { Command.SWITCH,       Switch },
            { Command.SAVE,         Save },
            { Command.HELP,         Help },
        };
        
        public static IEnumerable<Command> GetGlobalCommands()
        {
            return GlobalCommands.Keys.ToArray();
        }
        
        protected Interpreter()
        {
            OnSwitchMode += SwitchMode;
        }

        public void Enable()
        {
            //Debug.LogInfo($"{Mode()} interpreter has been enabled!");
            Terminal.OnCommandBroadcast += Interpret;
            Start();
        }

        public void Disable()
        {
            //Debug.LogInfo($"{Mode()} interpreter has been disabled!");
            Terminal.OnCommandBroadcast -= Interpret;
        }

        protected virtual void Start() { }

        private void SwitchMode(MediaSpace.Mode newMode)
        {
            /*if (Mode() == newMode)
            {
                Enable();
            }
            else
            {
                Disable();
            }*/
        }

        public abstract MediaSpace.Mode Mode();

        public abstract IEnumerable<Command> GetCommands();
        
        public static T Parse<T>(string command) => (T)Enum.Parse(typeof(T), command.ToUpper());

        public static bool TryParse<T>(string command, out T type) where T: struct => 
            Enum.TryParse(command.ToUpper(), out type);

        public static Action<MediaSpace.Mode> OnSwitchMode = delegate { };

        // TODO: Error Check for proper number of arguments and error logging if not
        // TODO: Add verbose error logging
        private void Interpret(string cmd, int argc, string[] argv)
        {
            if (TryParse(cmd, out Command command))
            {
                if (GlobalCommands.ContainsKey(command))
                {
                    GlobalCommands[command].Invoke(argc, argv);
                }
                else
                {
                    Invoke(command, argc, argv);
                }
            }
            else
            {
                Debug.LogError($"Unrecognised command: \"{cmd}\"");
                Debug.Log("Type \"HELP\" to see a list of valid commands");
            }
            
            Debug.LineBreak();
        }

        protected abstract void Invoke(Command command, int argc, string[] argv);
        
        #region Global Commands
        
        private static void Save(int argc, string[] argv)
        {
            if (argc == 0)
            {
                Program.Save();
                Debug.LogInfo("Saved all files");
            }
            else
            {
                Usage("\"SAVE\"");
            }
        }
        
        protected static void Exit(int argc, string[] argv)
        {
            if (Debug.Confirm("exit"))
            {
                Application.Shutdown();
                //Program.Exit(ExitCode.Normal);
            }
        }

        private static void Switch(int argc, string[] argv)
        {
            if (argc > 0 && TryParse(argv[0], out MediaSpace.Mode newMode))
            {
                OnSwitchMode.Invoke(newMode);
            }
            else
            {
                Invalid("Unable to switch (invalid MediaSpace)");   
            }
        }
        
        private static void Help(int argc, string[] argv)
        {
            if (argc > 0)
            {
                UserGuide.Help(Program.Data.Config.MediaSpaceMode, argv[0]);
            }
            else
            {
                UserGuide.General();
            }
        }
        
        #endregion Global Commands
        
        public static void Usage(string message)
        {
            Debug.LogCustom("USAGE", message, ConsoleColor.DarkYellow);
        }

        protected static void Invalid(string additionalInfo = null, [CallerLineNumber] int lineNumber = 0, 
            [CallerFilePath] string filepath = "")
        {
            Debug.LogError($"Command failed (line {lineNumber} in \"{Path.GetFileName(filepath)}\")");
            if (additionalInfo != null)
            {
                Debug.Log($"Additional information: {additionalInfo}");
            }
        }
        
        protected static void DeprecatedWarning()
        {
            Debug.LogWarning("This method has been deprecated and will be removed in a future version");
        }

        protected static void Unimplemented(int argc, string[] argv)
        {
            Debug.LogWarning("Command not implemented in this MediaSpace");
        }

        protected static void WrongMediaSpace(int argc, string[] argv)
        {
            Debug.LogWarning($"This MediaSpace ({Program.Data.Config.MediaSpaceMode}) is not configured for this command. Please switch MediaSpaces using the SWITCH command and try again.");
        }
    }
}