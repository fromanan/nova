using System;
using System.IO;
using NovaCore.Library.Files;
using NovaCore.Library.Utilities;

namespace NovaCore.Library
{
    public static class Program
    {
        public enum ExitCode : int 
        {
            Success = 0,
            Error = 1,
            Critical = 2,
            Crash = 3,
            Normal = 4
        }
        
        public static class Paths
        {
            // Project Specific
            public static readonly string Root = Info.ApplicationTitle;
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