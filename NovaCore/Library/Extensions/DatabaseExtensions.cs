using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using NovaCore.Utilities;
using NovaCore.Web;
using Debug = NovaCore.Logging.Debug;

namespace NovaCore.Extensions
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

        private static string Decode(object item) => WebUtils.Decode($"{item}");
        
        public static string Format(this DataRow dataRow, int colLength, string separator = " ")
        {
            return dataRow.ItemArray.
                Select(item => Decode(item).Truncate(colLength-3).PadRight(colLength)).
                Merge(separator);
        }
        
        public static string Format(this DataRow dataRow, string separator = " ")
        {
            return dataRow.ItemArray.Select(Decode).Merge(separator);
        }

        public static void Print(this DataRow dataRow)
        {
            Debug.Log(dataRow.Format());
        }

        public static string FormatColumns(this DataTable dataTable, int colLength = -1, string separator = " ")
        {
            return dataTable.GetColumns().FormatColumns(colLength, separator);
        }
        
        public static string FormatColumns(this IEnumerable<DataColumn> columns, int colLength = -1, string separator = " ")
        {
            return columns.
                Select(col => colLength < 0 ? col.ColumnName : col.ColumnName.PadRight(colLength)).
                Merge(separator);
        }
        
        public static string FormatRows(this DataTable dataTable, int colLength = -1, string separator = "\n")
        {
            return dataTable.GetRows().FormatRows(colLength, separator);
        }
        
        public static string FormatRows(this IEnumerable<DataRow> rows, int colLength = -1, string separator = "\n")
        {
            return rows.
                Select(dataRow => colLength < 0 ? dataRow.Format() : dataRow.Format(colLength)).
                Merge(separator);
        }
        
        public static void Print(this DataTable dataTable)
        {
            Debug.Log(dataTable.FormatRows());
        }
        
        public static void Print(this DataTable dataTable, int colLength, int count = 0, int start = 0)
        {
            StringBuilder buffer = new StringBuilder();
            
            // Print Headers
            buffer.AppendLine(dataTable.FormatColumns(colLength));

            // Print Separators
            buffer.AppendLine(Separators(colLength, dataTable.Columns.Count));

            // Print the Data Elements
            IEnumerable<DataRow> rows = dataTable.GetRows();
            if (start > 0) rows = rows.Skip(start);
            if (count > 0) rows = rows.Take(count);
            buffer.Append(rows.FormatRows(colLength));

            Debug.Log(buffer.ToString());
        }

        private static string Separators(int colLength, int colCount, string separator = " ")
        {
            return Enumerable.Repeat(new string('=', colLength), colCount).Merge(separator);
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
        // Converted to modern C# (8) and simplified
        public static T ToObject<T>(this DataRow dataRow) where T : new()
        {
            T item = new T();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                if (GetProperty(typeof(T), column.ColumnName) is PropertyInfo property && 
                    dataRow[column] != DBNull.Value && dataRow[column].ToString() != "NULL")
                {
                    property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
                }
            }

            return item;
        }

        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            return type.GetProperty(attributeName) ?? type.GetProperties().FirstOrDefault(p => Sigma(p, attributeName));
        }

        private static bool Sigma(PropertyInfo propertyInfo, string attributeName)
        {
            return propertyInfo.IsDefined(typeof(DisplayAttribute), false) &&
                   propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), false)
                       .Cast<DisplayAttribute>().Single().Name == attributeName;
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