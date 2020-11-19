using System;

namespace Nova.Library.Classes
{
    public static class Flags
    {
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
            int flag = 0;
            foreach (Enum state in enums)
            {
                flag |= EnumToFlag(state);
            }

            return flag;
        }
    }
}