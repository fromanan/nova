using System;
using System.Collections.Generic;
using NovaCore.Core;
using Debug = NovaCore.Logging.Debug;

namespace NovaCore.CLI.Interpreters
{
    public class InterpreterCollection
    {
        public readonly Dictionary<MediaSpace.Mode, Interpreter> Interpreters = new();

        public bool InterpreterExists(MediaSpace.Mode mode) => Interpreters.ContainsKey(mode);

        public static MediaSpace.Mode CurrentMediaSpace
        {
            get => Program.Data.Config.MediaSpaceMode;
            set => Program.Data.Config.MediaSpaceMode = value;
        }

        public Interpreter GetActiveInterpreter()
        {
            if (InterpreterExists(CurrentMediaSpace)) return Interpreters[CurrentMediaSpace];
            Debug.LogCritical("Active interpreter not found");
            return null;
        }

        public IEnumerable<Command> GetCurrentCommands() => GetActiveInterpreter().GetCommands();

        public void Init()
        {
            // Startup message
            Debug.LogInfo($"Global MediaSpace set to \"{CurrentMediaSpace}\"");
            
            // Create the initial loaded interpreter
            CreateNewInterpreterInstance(CurrentMediaSpace);
            
            // Enable the Interpreter
            GetActiveInterpreter().Enable();
                
            // Ensure that a new interpreter will be created if none exists when switching
            Interpreter.OnSwitchMode += SwitchActiveInterpreter;
        }

        public void SwitchActiveInterpreter(MediaSpace.Mode newMode)
        {
            if (newMode == CurrentMediaSpace)
            {
                Debug.LogWarning($"Global MediaSpace is already \"{newMode}\"");
                return;
            }
            Debug.LogInfo($"Switching global MediaSpace mode to \"{newMode}\"");
            GetActiveInterpreter().Disable();
            CurrentMediaSpace = newMode;
            CreateNewInterpreterInstance(newMode);
            GetActiveInterpreter().Enable();
            NovaApp.Save();
        }
        
        public void CreateNewInterpreterInstance(MediaSpace.Mode mode)
        {
            if (InterpreterExists(mode)) return;
            switch (mode)
            {
                case MediaSpace.Mode.GENERAL:
                    Interpreters[mode] = new GeneralInterpreter();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}