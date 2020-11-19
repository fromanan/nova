using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Nova.Library.Utilities.Functions
{
    public static class Hash
    {
        // https://stackoverflow.com/questions/16999361/obtain-sha-256-string-of-a-string/17001289
        public static string Sha256(string value)
        {
            StringBuilder sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
        
        private static readonly Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string SaltedHash(string text, string salt)
        {
            return Sha256($"{text}{salt}");
        }
    }
}