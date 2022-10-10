using System;

namespace NovaCore.Common
{
    public static class NovaApp
    {
        public static readonly Logging.Logger Logger = new();

        public static void OnCancel()
        {
            Global.Shutdown();
        }
        
        // Safe Exit of Program (Events are called)
        public static void OnShutdown()
        {
            Logger.LogInfo($"{AppInfo.ProductName} Closed ({DateTime.Now:G})");
        }

        // Immediate Exit of Application/Program
        public static void OnExit(ExitCode exitCode = ExitCode.Success)
        {
            Logger.Log($"Program Terminating | Exit Code: {(int)exitCode} ({exitCode.ToString()})");
            Environment.Exit((int)exitCode);
        }
    }
}