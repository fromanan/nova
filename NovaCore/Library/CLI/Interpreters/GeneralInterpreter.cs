using System;
using System.Collections.Generic;
using System.Linq;
using NovaCore.Core;
using static NovaCore.Files.FileSystem;
using Debug = NovaCore.Logging.Debug;

namespace NovaCore.CLI.Interpreters
{
    public class GeneralInterpreter : Interpreter
    {
        private static readonly CommandDictionary Commands = new CommandDictionary
        {
            { Command.OPEN,         Open },
            { Command.SET,          Set },
            { Command.GET,          Get },
            { Command.CLEAR,        Clear },
        };

        public override MediaSpace.Mode Mode() => MediaSpace.Mode.GENERAL;

        public override IEnumerable<Command> GetCommands()
        {
            return Commands.Keys.ToArray();
        }

        protected override void Invoke(Command command, int argc, string[] argv)
        {
            if (!Commands.ContainsKey(command))
            {
                WrongMediaSpace(argc, argv);
                return;
            }
            
            Commands[command].Invoke(argc, argv);
        }

        private static void Open(int argc, string[] argv)
        {
            if (argc > 0)
            {
                switch (argv[0].ToUpper())
                {
                    case "FILE":
                        if (argc > 2 && argv[2].ToUpper() == "REVEAL")
                        {
                            ShowFileLocation(argv[1]);
                        }
                        else
                        {
                            OpenWithDefaultProgram(argv[1]);
                        }
                        break;
                    case "FOLDER":
                        OpenFolder(argv[1]);
                        break;
                    default:
                        Invalid();
                        break;
                }
            }
            else
            {
                Invalid();
            }
        }

        private static void Set(int argc, string[] argv)
        {
            if (argc > 0)
            {
                switch (argv[0].ToUpper())
                {
                    case "DEFAULT_DOWNLOAD_PATH":
                        if (argc > 1) // Set value
                        {
                            string path = argv[1];
                            if (!Validate(path))
                            {
                                Invalid();
                                return;
                            }
                            Program.Data.Config.SetDefaultSavePath(argv[1]);
                        }
                        else // Reset Value
                        {
                            Program.Data.Config.SetDefaultSavePath();
                        }
                        break;
                    case "PREF":
                    case "PREFERENCE":
                        switch (argc)
                        {
                            // SET PREF [KEY] => Delete Key
                            case 2:
                                Debug.LogInfo($"Removed key \"{argv[1]}\" from preferences");
                                Program.Preferences.DeleteKey(argv[1]);
                                break;
                            // SET PREF [KEY] [VALUE] => Insert string preference
                            case 3:
                                Debug.LogInfo($"Set preference key \"{argv[1]}\" to \"{argv[2]}\" (string)");
                                Program.Preferences.SetString(argv[1], argv[2]);
                                break;
                            // SET PREF [KEY] [VALUE] [TYPE] => Insert type preference
                            case 4:
                                switch (argv[3].ToUpper())
                                {
                                    case "INT":
                                        Debug.LogInfo($"Set preference key \"{argv[1]}\" to {argv[2]} (int)");
                                        Program.Preferences.SetInt(argv[1], Convert.ToInt32(argv[2]));
                                        break;
                                    case "FLOAT":
                                        Debug.LogInfo($"Set preference key \"{argv[1]}\" to {argv[2]} (float)");
                                        Program.Preferences.SetFloat(argv[1], Convert.ToInt64(argv[2]));
                                        break;
                                }
                                break;
                            default:
                                Invalid();
                                break;
                        }
                        break;
                    default:
                        Invalid();
                        break;
                }
            }
            else
            {
                Invalid();
            }
        }
        
        private static void Get(int argc, string[] argv)
        {
            if (argc > 0)
            {
                switch (argv[0].ToUpper())
                {
                    case "DEFAULT_DOWNLOAD_PATH":
                    {
                        Debug.Log(Program.Data.Config.DefaultSavePath);
                        break;
                    }
                    case "PREF":
                    case "PREFERENCE":
                    {
                        string key;
                        switch (argc)
                        {
                            // GET PREF [KEY] => Get string preference
                            case 2:
                                key = argv[1];
                                if (!Program.Preferences.Exists(key))
                                {
                                    Debug.LogWarning($"Preference key \"{key}\" does not exist");
                                    break;
                                }
                                Debug.Log($"{key} = \"{Program.Preferences.GetString(key)}\"");
                                break;
                            // GET PREF [KEY] [TYPE] => Insert type preference
                            case 3:
                                key = argv[1];
                                if (!Program.Preferences.Exists(key))
                                {
                                    Debug.LogWarning($"Preference key \"{key}\" does not exist");
                                    break;
                                }
                                switch (argv[2].ToUpper())
                                {
                                    case "INT":
                                        Debug.Log($"{key} = {Program.Preferences.GetInt(key)}");
                                        break;
                                    case "FLOAT":
                                        Debug.Log($"{key} = {Program.Preferences.GetFloat(key):4f}");
                                        break;
                                }
                                break;
                            default:
                                Invalid();
                                break;
                        }
                        break;
                    }
                    default:
                    {
                        Invalid();
                        break;
                    }
                }
            }
            else
            {
                Invalid();
            }
        }

        private static void Clear(int argc, string[] argv)
        {
            if (argc > 0)
            {
                switch (argv[0].ToUpper())
                {
                    case "DEFAULT_DOWNLOAD_PATH":
                    {
                        if (argc == 1) // Set value
                        {
                            Program.Data.Config.SetDefaultSavePath();
                        }
                        else
                        {
                            Invalid();
                        }
                        break;
                    }
                    case "PREF":
                    case "PREFERENCE":
                    {
                        switch (argc)
                        {
                            // CLEAR PREF [KEY] => Delete Key
                            case 2:
                                Debug.LogInfo($"Removed key \"{argv[1]}\" from preferences");
                                Program.Preferences.DeleteKey(argv[1]);
                                break;
                            default:
                                Invalid();
                                break;
                        }
                        break;
                    }
                    default:
                    {
                        Invalid();
                        break;
                    }
                }
            }
            else
            {
                Invalid();
            }
        }
    }
}