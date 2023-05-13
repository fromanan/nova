using System;
using System.Diagnostics;
using System.Reflection;
using NovaCore.Common.Resources;

namespace NovaCore.Common;

public static class AppInfo
{
    public static readonly Assembly AssemblyInfo = Assembly.GetEntryAssembly();
        
    public static readonly AssemblyName AssemblyName = AssemblyInfo?.GetName();
        
    public static readonly FileVersionInfo VersionInfo = FileVersionInfo.GetVersionInfo(AppContext.BaseDirectory);

    //Assembly.GetExecutingAssembly().FullName
    //Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
    public static string ApplicationTitle => AssemblyInfo?.FullName;
        
    public static string ProductName => VersionInfo.ProductName;

    public static string ProductVersion => VersionInfo.ProductVersion;

    public static string CompanyName => VersionInfo.CompanyName;

    public static readonly string Copyright = 
        string.Format(Messages.Copyright, ProductName, ProductVersion, CompanyName, DateTime.Now.Year);
}