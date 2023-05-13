using System;
using System.Configuration;

namespace NovaCore.Common.Utilities;

public static class Data
{
    public static string GetValue(string key)
    {
        return ConfigurationManager.AppSettings.Get(key);
    }
    
    public static bool ToBool(object value) => Convert.ToBoolean(Convert.ToInt32(value));

    public static int ToInt(object value) => Convert.ToInt32(value);

    public static long ToLong(object value) => Convert.ToInt64(value);

    public static float ToFloat(object value) => Convert.ToSingle(value);

    public static double ToDouble(object value) => Convert.ToDouble(value);
}