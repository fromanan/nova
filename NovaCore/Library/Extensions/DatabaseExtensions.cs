using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using NovaCore.Library.Logging;
using NovaCore.Library.Utilities;
using NovaCore.Library.Web;

namespace NovaCore.Library.Extensions
{
    public static class DatabaseExtensions
    {
        public static IEnumerable<DataRow> GetRows(this DataTable dataTable)
        {
            return dataTable.Rows.Cast<DataRow>();
        }
        
        public static IEnumerable<DataColumn> GetColumns(this DataTable dataTable)
        {
            return dataTable.Columns.Cast<DataColumn>();
        }

        private static string Join(string separator, IEnumerable<string> values) => string.Join(separator, values);

        private static string Decode(object item) => WebUtils.Decode($"{item}");
        
        public static string Format(this DataRow dataRow, int colLength, string separator = " ")
        {
            return Join(separator, 
                dataRow.ItemArray.Select(item => Decode(item).Truncate(colLength-3).PadRight(colLength)));
        }
        
        public static string Format(this DataRow dataRow, string separator = " ")
        {
            return Join(separator, dataRow.ItemArray.Select(Decode));
        }

        public static void Print(this DataRow dataRow)
        {
            Debug.Log(dataRow.Format());
        }
        
        public static string Format(this DataTable dataTable, int colLength, string separator = "\n")
        {
            return Join(separator, dataTable.GetRows().Select(dataRow => dataRow.Format(colLength)));
        }
        
        public static string Format(this DataTable dataTable, string separator = "\n")
        {
            return Join(separator, dataTable.GetRows().Select(dataRow => dataRow.Format()));
        }
        
        public static void Print(this DataTable dataTable)
        {
            Debug.Log(dataTable.Format());
        }

        public static void Print(this DataTable dataTable, int colLength)
        {
            // Print Headers
            Debug.Log(Join(" ", dataTable.GetColumns().Select(col => col.ColumnName.PadRight(colLength))));

            // Print Separators
            Debug.Log(Join(" ", Enumerable.Repeat(new string('=', colLength), dataTable.Columns.Count)));

            // Print the Data Elements
            Debug.Log(dataTable.Format(colLength));
        }

        public static void DropColumn(this DataTable dataTable, string columnName)
        {
            dataTable.Columns.Remove(columnName);
        }
        
        public static void DropColumns(this DataTable dataTable, params string[] columnNames)
        {
            columnNames.ToList().ForEach(dataTable.Columns.Remove);
        }
        
        // Source: https://stackoverflow.com/questions/19673502/how-to-convert-datarow-to-an-object/45074265
        public static T ToObject<T>(this DataRow dataRow) where T : new()
        {
            T item = new T();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo property = GetProperty(typeof(T), column.ColumnName);

                if (property != null && dataRow[column] != DBNull.Value && dataRow[column].ToString() != "NULL")
                {
                    property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
                }
            }

            return item;
        }

        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);

            if (property != null)
            {
                return property;
            }

            return type
                .GetProperties()
                .FirstOrDefault(p => p.IsDefined(typeof(DisplayAttribute), false) && 
                                     p.GetCustomAttributes(typeof(DisplayAttribute), false)
                                         .Cast<DisplayAttribute>().Single().Name == attributeName);
        }

        public static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return value == null ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(type) ?? typeof(object));
            }

            return Convert.ChangeType(value, type);
        }

        public static int GetInt(this DataRow dataRow, string key)
        {
            return Utils.ToInt(dataRow[key]);
        }

        public static bool GetBool(this DataRow dataRow, string key)
        {
            return Utils.ToBool(dataRow[key]);
        }

        public static string GetString(this DataRow dataRow, string key)
        {
            return dataRow[key].ToString();
        }

        public static string GetEncodedString(this DataRow dataRow, string key)
        {
            return WebUtils.Decode(dataRow[key].ToString());
        }
    }
}