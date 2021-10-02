using System;
using System.IO;
using System.Windows.Forms;
using NovaCore.Library.Files;
using NovaCore.Library.Logging;

namespace NovaCore.Library
{
    public partial class Program
    {
        public static class Paths
        {
            // Project Specific
            public static readonly string Root = Application.ProductName;
            public static readonly string Settings = Path.Combine(FileSystem.Paths.AppData, Root, @"Settings");
            public static readonly string Preferences = Path.Combine(FileSystem.Paths.AppData, Root, @"Settings");
            public static readonly string UserData = Path.Combine(FileSystem.Paths.AppData, Root, @"UserData");
            public static readonly string Log = Path.Combine(FileSystem.Paths.AppData, Root, @"Logfiles");
        }
        
        public static void Exit(ExitCode exitCode)
        {
            Environment.Exit((int)exitCode);
        }
    }
}