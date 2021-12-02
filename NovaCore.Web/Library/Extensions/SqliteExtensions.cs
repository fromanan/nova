using Microsoft.Data.Sqlite;
using Debug = NovaCore.Logging.Debug;

namespace NovaCore.Web.Extensions
{
    public static class SqliteExtensions
    {
        public static void EnumerateResults(this SqliteDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Debug.Log($"- {reader.GetName(i)} : {reader.GetValue(i)}");
            }
        }
    }
}