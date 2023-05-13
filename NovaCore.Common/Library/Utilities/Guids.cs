using System;

namespace NovaCore.Common.Utilities;

public static class Guids 
{
    public static Guid New() => Guid.NewGuid();

    public static Guid Load(string guid) => Guid.Parse(guid);
    
    // https://stackoverflow.com/questions/4825907/convert-int-to-guid
    public static Guid Int2Guid(int value) => new(BitConverter.GetBytes(value));

    public static int Guid2Int(Guid value) => BitConverter.ToInt32(value.ToByteArray(), 0);
}