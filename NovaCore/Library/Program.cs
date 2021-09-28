using System;
using System.IO;
using NovaCore.Library.Files;
using NovaCore.Library.Logging;

namespace NovaCore.Library
{
    public partial class Program
    {
        public static class Paths
        {
            // Project Specific
            public const string ROOT = @"Informatix";
            public static readonly string Settings = Path.Combine(FileSystem.Paths.AppData, ROOT, @"Settings");
            public static readonly string Preferences = Path.Combine(FileSystem.Paths.AppData, ROOT, @"Settings");
            public static readonly string UserData = Path.Combine(FileSystem.Paths.AppData, ROOT, @"UserData");
            public static readonly string Log = Path.Combine(FileSystem.Paths.AppData, ROOT, @"Logfiles");
        }
        
        public static void Exit(ExitCode exitCode)
        {
            Environment.Exit((int)exitCode);
        }
    }
}