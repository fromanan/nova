using System;

namespace Nova.Library.Utilities.Functions
{
    public static class GUID 
    {
        public static Guid Generate()
        {
            return Guid.NewGuid();
        }

        public static Guid Load(string guid)
        {
            return Guid.Parse(guid);
        }
    
        // https://stackoverflow.com/questions/4825907/convert-int-to-guid
        public static Guid Int2Guid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static int Guid2Int(Guid value)
        {
            byte[] b = value.ToByteArray();
            int bint = BitConverter.ToInt32(b, 0);
            return bint;
        }
    }
}