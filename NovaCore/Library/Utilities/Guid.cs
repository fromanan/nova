using System;

namespace NovaCore.Library.Utilities
{
    public static class Guid 
    {
        public static System.Guid Generate()
        {
            return System.Guid.NewGuid();
        }

        public static System.Guid Load(string guid)
        {
            return System.Guid.Parse(guid);
        }
    
        // https://stackoverflow.com/questions/4825907/convert-int-to-guid
        public static System.Guid Int2Guid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new System.Guid(bytes);
        }

        public static int Guid2Int(System.Guid value)
        {
            byte[] b = value.ToByteArray();
            int bint = BitConverter.ToInt32(b, 0);
            return bint;
        }
    }
}