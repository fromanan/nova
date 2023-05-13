using System;
using System.IO;

namespace NovaCore.Common.Utilities;

public static class Paths
{
    // System Folders
    public static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public static readonly string Common = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
    public static readonly string Local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    public static readonly string Temp = Path.GetTempPath();
    public static readonly string Documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    public static readonly string Downloads = KnownFolders.GetPath(KnownFolder.Downloads);

    // Project Specific
    public static readonly string Root = Data.GetValue("ApplicationName");
    public static readonly string Build = AppContext.BaseDirectory;
    public static readonly string Project = Path.Combine(Environment.CurrentDirectory, "..", "..", "..");
    public static readonly string Settings = Path.Combine(AppData, Root, @"Settings");
    public static readonly string Preferences = Path.Combine(AppData, Root, @"Settings");
    public static readonly string UserData = Path.Combine(AppData, Root, @"UserData");
    public static readonly string Log = Path.Combine(AppData, Root, @"Logfiles");
    public static readonly string Resources = Path.Combine(Project, "resources");
    public static readonly string External = Path.Combine(Project, "External");
    public static readonly string ProjectDownloads = Path.Combine(KnownFolders.GetPath(KnownFolder.Downloads), Root);
}