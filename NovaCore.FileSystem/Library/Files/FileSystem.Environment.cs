using System;
using System.Collections;

namespace NovaCore.Files;

public static partial class FileSystem
{
    public static void SetEnvironmentVariable(string key, string value, bool overwrite = true)
    {
        // Check whether the environment variable exists.
        string currentValue = Environment.GetEnvironmentVariable(key);
            
        // If necessary, create it.
        if (string.IsNullOrEmpty(currentValue) || overwrite)
        {
            Environment.SetEnvironmentVariable(key, value);
        }
    }
        
    public static string GetEnvironmentVariable(string key)
    {
        return Environment.GetEnvironmentVariable(key);
    }
        
    public static IDictionary GetEnvironmentVariables(string key)
    {
        return Environment.GetEnvironmentVariables();
    }

    public static void DeleteEnvironmentVariable(string key)
    {
        Environment.SetEnvironmentVariable(key, null);
    }
}