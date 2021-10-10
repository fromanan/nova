using System.Reflection;

namespace NovaCore.Library.Utilities
{
    public static class Info
    {
        public static string ApplicationTitle => 
            Assembly.GetCallingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
    }
}