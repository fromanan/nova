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
        private static readonly CommandDictionary Commands = new()
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

        private static class Keywords
        {
            public const string PREF = "PREF";
            public const string PREFS = "PREFS";
            public const string PREFERENCE = "PREFERENCE";
            public const string PREFERENCES = "PREFERENCES";
            public const string DEFAULT_DOWNLOAD_PATH = "DEFAULT_DOWNLOAD_PATH";
            public const string FILE = "FILE";
            public const string FOLDER = "FOLDER";
        }

        private static void Open(int argc, string[] argv)
        {
            if (argc > 0)
            {
                switch (argv[0].ToUpper())
                {
                    case Keywords.FILE:
                        if (argc > 2 && argv[2].ToUpper() == "REVEAL")
                        {
                            ShowFileLocation(argv[1]);
                        }
                        else
                        {
                            OpenWithDefaultProgram(argv[1]);
                        }
                        break;
                    case Keywords.FOLDER:
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
                    case Keywords.DEFAULT_DOWNLOAD_PATH:
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
                    case Keywords.PREF:
                    case Keywords.PREFERENCE:
                        switch (argc)
                        {
                            // SET PREF [KEY] => Delete Key
                            case 2:
                                Logger.LogInfo($"Removed key \"{argv[1]}\" from preferences");
                                Program.Preferences.DeleteKey(argv[1]);
                                break;
                            // SET PREF [KEY] [VALUE] => Insert string preference
                            case 3:
                                Logger.LogInfo($"Set preference key \"{argv[1]}\" to \"{argv[2]}\" (string)");
                                Program.Preferences.SetString(argv[1], argv[2]);
                                break;
                            // SET PREF [KEY] [VALUE] [TYPE] => Insert type preference
                            case 4:
                                switch (argv[3].ToUpper())
                                {
                                    case "INT":
                                        Logger.LogInfo($"Set preference key \"{argv[1]}\" to {argv[2]} (int)");
                                        Program.Preferences.SetInt(argv[1], Convert.ToInt32(argv[2]));
                                        break;
                                    case "FLOAT":
                                        Logger.LogInfo($"Set preference key \"{argv[1]}\" to {argv[2]} (float)");
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
                    case Keywords.DEFAULT_DOWNLOAD_PATH:
                    {
                        Logger.Log(Program.Data.Config.DefaultSavePath);
                        break;
                    }
                    case Keywords.PREF:
                    case Keywords.PREFERENCE:
                    {
                        string key;
                        switch (argc)
                        {
                            // GET PREF [KEY] => Get string preference
                            case 2:
                                key = argv[1];
                                if (!Program.Preferences.Exists(key))
                                {
                                    Logger.LogWarning($"Preference key \"{key}\" does not exist");
                                    break;
                                }
                                Logger.Log($"{key} = \"{Program.Preferences.GetString(key)}\"");
                                break;
                            // GET PREF [KEY] [TYPE] => Insert type preference
                            case 3:
                                key = argv[1];
                                if (!Program.Preferences.Exists(key))
                                {
                                    Logger.LogWarning($"Preference key \"{key}\" does not exist");
                                    break;
                                }
                                switch (argv[2].ToUpper())
                                {
                                    case "INT":
                                        Logger.Log($"{key} = {Program.Preferences.GetInt(key)}");
                                        break;
                                    case "FLOAT":
                                        Logger.Log($"{key} = {Program.Preferences.GetFloat(key):4f}");
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
                    case Keywords.DEFAULT_DOWNLOAD_PATH:
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
                    case Keywords.PREF:
                    case Keywords.PREFERENCE:
                    {
                        switch (argc)
                        {
                            // CLEAR PREF [KEY] => Delete Key
                            case 2:
                                Logger.LogInfo($"Removed key \"{argv[1]}\" from preferences");
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