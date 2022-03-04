using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using NovaCore.CLI;
using NovaCore.Extensions;

namespace NovaCore.Common
{
    public static class NovaApp
    {
        public static readonly Logger Logger = new();

        private static readonly char Separator = Path.PathSeparator;
        
        public static class Paths
        {
            // System Path
            private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            
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
        }
        
        public static string Copyright()
        {
            StringBuilder buffer = new();
            buffer.AppendLine($"{AppInfo.ProductName}, Version {AppInfo.ProductVersion}");
            buffer.AppendLine($"Copyright {AppInfo.CompanyName} ({DateTime.Now.Year}). All rights reserved.");
            return buffer.ToString();
        }

        public static Terminal CurrentTerminal;

        public static void OpenTerminal()
        {
            CurrentTerminal = new Terminal(Logger);
            CurrentTerminal.Run();
            CurrentTerminal.Close();
            CurrentTerminal = null;
        }

        public static void Init()
        {
            // Shutdown Events
            AppDomain.CurrentDomain.ProcessExit += OnExit;
            Console.CancelKeyPress += OnCancel;
            
            InitEvent.Invoke();
        }
        
        public static void Validate()
        {
            ValidateEvent.Invoke();
        }

        public static void Save()
        {
            SaveEvent.Invoke();
        }

        public static void Load()
        {
            LoadEvent.Invoke();
        }
        
        // Standard Shutdown Procedure for Application
        public static void Close()
        {
            Save();
            Shutdown();
            Logger.LogInfo($"{AppInfo.ProductName} Closed ({DateTime.Now:G})");
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

        public static void Crash(string message = null, Exception exception = null)
        {
            Environment.FailFast(message, exception);
        }
        
        public static void Restart()
        {
            RestartEvent.Invoke();
            Exit(ExitCode.Restart);
        }
        
        // Corresponds to the ProcessExit event
        private static void OnExit(object sender, EventArgs e)
        {
            Shutdown();
        }

        private static void OnCancel(object sender, EventArgs e)
        {
            CancelEvent.Invoke();   
        }
        
        [DllImport("kernel32.dll", SetLastError = true)]
        //public static extern bool AttachConsole(uint dwProcessId);
        public static extern bool AttachConsole(int pid);
        
        [DllImport("kernel32")]
        public static extern bool AllocConsole();
        
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int nStdHandle);
        
        public static bool RunAsConsole()
        {
            // Check if Console exists, if not, attach it
            return AttachConsole(-1) || AllocConsole();
        }

        public static IntPtr StandardOutputHandle => GetStdHandle(-11);
        
        public static SafeFileHandle StandardOutputHandleSafe => new(GetStdHandle(-11), false);

        public static StreamWriter GetConsoleWriter()
        {
            return new StreamWriter(new FileStream(StandardOutputHandleSafe, FileAccess.Write));
        }

        public static event Action InitEvent = delegate {  };
        
        public static event Action ValidateEvent = delegate {  };
        
        public static event Action ExitEvent = delegate {  };
        
        public static event Action ShutdownEvent = delegate {  };
        
        public static event Action SaveEvent = delegate {  };
        
        public static event Action LoadEvent = delegate {  };
        
        public static event Action RestartEvent = delegate {  };
        
        public static event Action CancelEvent = delegate {  };
    }
}