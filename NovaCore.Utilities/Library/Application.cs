using System;
using System.IO;
using System.Text;
using NovaCore.Common;
using NovaCore.Extensions;
using NovaCore.Utilities;
using App = System.Windows.Forms.Application;

namespace NovaCore
{
    public static class Application
    {
        public static readonly Logger Logger = new Logger();

        private static char Separator = Path.PathSeparator;
        
        public static class Paths
        {
            // Project Specific
            public static readonly string Root = AppInfo.ProductName.Remove(" ");
            public static readonly string Build = AppContext.BaseDirectory;
            public static readonly string Project = Path.Combine(Environment.CurrentDirectory, "..", "..", "..");
            public static readonly string Settings = Path.Combine(AppData, Root, @"Settings");
            public static readonly string Preferences = Path.Combine(AppData, Root, @"Settings");
            public static readonly string UserData = Path.Combine(AppData, Root, @"UserData");
            public static readonly string Log = Path.Combine(AppData, Root, @"Logfiles");
            public static readonly string Resources = Path.Combine(Project, "resources");
            public static readonly string External = Path.Combine(Project, "External");
            public static readonly string Downloads = Path.Combine(KnownFolders.GetPath(KnownFolder.Downloads), Root);
            private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }
        
        public static string Copyright()
        {
            StringBuilder buffer = new();
            buffer.AppendLine($"{AppInfo.ProductName}, Version {AppInfo.ProductVersion}");
            buffer.AppendLine($"Copyright {AppInfo.CompanyName} ({DateTime.Now.Year}). All rights reserved.");
            return buffer.ToString();
        }
        
        // Safe Exit of Program (Events are called)
        public static void Shutdown()
        {
            ShutdownEvent.Invoke();
        }

        // Immediate Exit of Application/Program
        public static void Exit(ExitCode exitCode = ExitCode.Success)
        {
            Logger.Log($"Program Terminating | Exit Code: {(int)exitCode} ({exitCode.ToString()})");
            ExitEvent.Invoke();
            Environment.Exit((int)exitCode);
        }
        
        public static void Restart()
        {
            RestartEvent.Invoke();
            System.Windows.Forms.Application.Restart();
            Exit(ExitCode.Restart);
        }
        
        public static event Action InitEvent = delegate {  };
        
        public static event Action ValidateEvent = delegate {  };
        
        public static event Action ExitEvent = delegate {  };
        
        public static event Action ShutdownEvent = delegate {  };
        
        public static event Action SaveEvent = delegate {  };
        
        public static event Action LoadEvent = delegate {  };
        
        public static event Action RestartEvent = delegate {  };
    }
}