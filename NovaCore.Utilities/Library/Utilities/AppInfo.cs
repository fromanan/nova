using System.Reflection;

namespace NovaCore.Utilities
{
    using Application = System.Windows.Forms.Application;
    
    public static class AppInfo
    {
        //Assembly.GetExecutingAssembly().FullName
        //Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
        public static string ApplicationTitle => Assembly.GetEntryAssembly()?.FullName;
        
        public static string ProductName => Application.ProductName;

        public static string ProductVersion => Application.ProductVersion;

        public static string CompanyName => Application.CompanyName;
    }
}