using System;
using NovaCore.Common;
using Debug = NovaCore.Logging.Debug;

namespace NovaCore
{
    partial class Program
    {
        public static void Main(string[] args)
        {
            NovaApp.InitEvent += Init;

            Debug.SubscribeToDefault(NovaApp.Logger);
            
            NovaApp.Init();

            NovaApp.OpenTerminal();

            NovaApp.Save();
        }
        
        public static void Init()
        {
            Debug.Clear();
            
            Debug.Log(NovaApp.Copyright(), ConsoleColor.Cyan);

            Debug.Header("Initializing");

            Debug.Separator(true);
        }
    }
}