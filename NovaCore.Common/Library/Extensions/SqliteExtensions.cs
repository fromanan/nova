using System.Text;
using Microsoft.Data.Sqlite;

namespace NovaCore.Common.Extensions
{
    public static class SqliteExtensions
    {
        public static string EnumerateResults(this SqliteDataReader reader)
        {
            StringBuilder buffer = new();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                buffer.AppendLine($"- {reader.GetName(i)} : {reader.GetValue(i)}");
            }
            return buffer.ToString();
        }
    }
}