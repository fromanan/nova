using NovaCore.CLI.Interpreters;
using NovaCore.Configurations;

namespace NovaCore
{
    internal partial class Program
    {
        public static class Data
        {
            public static Config Config = new();
            public static readonly InterpreterCollection InterpreterCollection = new();

            public static void LoadAll()
            {
                Config = Config.Load<Config>();
                Preferences.LoadAll();
                InterpreterCollection.Init();
            }
            
            public static void SaveAll()
            {
                Config.Save();
                Preferences.SaveAll();
            }
        }
    }
}