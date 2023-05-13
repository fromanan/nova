using System;
using System.Security.Cryptography;
using System.Text;

namespace NovaCore.Common.Utilities;

public static class Hash
{
    // Source: https://stackoverflow.com/questions/16999361/obtain-sha-256-string-of-a-string/17001289
    // Simplified using LINQ
    public static string Sha256(string value)
    {
        using SHA256 sha256 = SHA256.Create();
        return Text.ByteString(sha256.ComputeHash(Encoding.UTF8.GetBytes(value)));
    }
        
    public static string Sha512(string value)
    {
        using SHA512 sha512 = SHA512.Create();
        return Text.ByteString(sha512.ComputeHash(Encoding.UTF8.GetBytes(value)));
    }

    public static string SimpleHash(string input)
    {
        using MD5 md5 = MD5.Create();
        return Text.ByteString(md5.ComputeHash(Encoding.ASCII.GetBytes(input)));
    }
        
    public static string SaltedHash(string text, string salt)
    {
        return Sha256($"{text}{salt}");
    }

    public static string ComputeHash(string value)
    {
        throw new NotImplementedException();
    }

    /*public static string ByteString<T>(string value, Encoding encoding) where T : HashAlgorithm
    {
        using (SHA512 hash = SHA512.Create())
        {
            return ByteString(hash.ComputeHash(encoding.GetBytes(value)));
        }
    }*/
}