using System;
using System.Collections.Generic;
using System.Linq;

namespace NovaCore.Classes
{
    public static class Flags
    {
        #region Legacy Code
        
        public static bool Contains(int flag, Enum state)
        {
            return (flag & EnumToFlag(state)) != 0;
        }

        public static int EnumToFlag(Enum state)
        {
            return 1 << Convert.ToInt32(state);
        }

        public static int ToFlag(params Enum[] enums)
        {
            return enums.Aggregate(0, (flag, state) => flag | EnumToFlag(state));
        }
        
        #endregion
        
        // Source: https://stackoverflow.com/questions/3261451/using-a-bitmask-in-c-sharp
        // The casts to object in the below code are an unfortunate necessity due to
        // C#'s restriction against a where T : Enum constraint. (There are ways around
        // this, but they're outside the scope of this simple illustration.)
        public static bool IsSet<T>(T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            return (flagsValue & flagValue) != 0;
        }

        public static void Set<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue | flagValue);
        }

        public static void Unset<T>(ref T flags, T flag) where T : struct
        {
            int flagsValue = (int)(object)flags;
            int flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue & (~flagValue));
        }

        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static string GetName<T>(T value) where T : Enum
        {
            return Enum.GetName(typeof(T), value);
        }
    }
}