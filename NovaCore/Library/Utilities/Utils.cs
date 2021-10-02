using System;

namespace NovaCore.Library.Utilities
{
    public static class Utils
    {
        public static bool ToBool(object value) => Convert.ToBoolean(Convert.ToInt32(value));

        public static int ToInt(object value) => Convert.ToInt32(value);
    }
}