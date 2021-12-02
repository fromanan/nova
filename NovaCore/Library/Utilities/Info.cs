using System.Reflection;

namespace NovaCore.Utilities
{
    public static class Info
    {
        public static string ApplicationTitle => 
            Assembly.GetCallingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
    }
}