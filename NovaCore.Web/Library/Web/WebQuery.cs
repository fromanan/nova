using System.Collections.Generic;

namespace NovaCore.Web
{
    public struct WebQuery
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public WebQuery(string key, string value)
        {
            Key = key;
            Value = value;
        }
        
        public static implicit operator KeyValuePair<string, string>(WebQuery webQuery)
        {
            return new KeyValuePair<string, string>(webQuery.Key, webQuery.Value);
        }

        public static explicit operator WebQuery(KeyValuePair<string, string> keyValuePair)
        {
            return new WebQuery(keyValuePair.Key, keyValuePair.Value);
        }
    }
}