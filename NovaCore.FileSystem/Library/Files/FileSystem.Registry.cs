#if WINDOWS
#pragma warning disable CA1416
using Microsoft.Win32;

namespace NovaCore.Files
{
    public static partial class FileSystem
    {
        public static void SetRegistryKey(string keyName, string valueName, string value)
        {
            Registry.SetValue(keyName, valueName, value);
        }
        
        public static void SetRegistryKey(string keyName, string valueName, string value, RegistryValueKind valueKind)
        {
            Registry.SetValue(keyName, valueName, value, valueKind);
        }

        public static T GetRegistryKey<T>(string keyName, string valueName, T defaultValue)
        {
            return (T) Registry.GetValue(keyName, valueName, defaultValue);
        }
    }
}
#pragma warning restore CA1416
#endif