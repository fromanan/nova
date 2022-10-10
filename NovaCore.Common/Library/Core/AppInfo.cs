using System;
using System.Diagnostics;
using System.Reflection;

namespace NovaCore.Common
{
    public static class AppInfo
    {
        public static readonly Assembly AssemblyInfo = Assembly.GetEntryAssembly();
        
        public static readonly AssemblyName AssemblyName = Assembly.GetEntryAssembly()?.GetName();
        
        public static readonly FileVersionInfo VersionInfo = FileVersionInfo.GetVersionInfo(AssemblyInfo.Location);

        //Assembly.GetExecutingAssembly().FullName
        //Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
        public static string ApplicationTitle => AssemblyInfo?.FullName;
        
        public static string ProductName => VersionInfo.ProductName;

        public static string ProductVersion => VersionInfo.ProductVersion;

        public static string CompanyName => VersionInfo.CompanyName;

        public static string Copyright = $"{ProductName}, Version {ProductVersion}\nCopyright {CompanyName} ({DateTime.Now.Year}). All rights reserved.";
    }
}