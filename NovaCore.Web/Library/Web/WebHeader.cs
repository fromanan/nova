using System;
using Header = System.Collections.Generic.KeyValuePair<string, string>;

namespace NovaCore.Web
{
    public class WebHeader : IFormattable
    {
        private readonly Header header;

        public string Key => header.Key;

        public string Value => header.Value;

        public WebHeader(Header header)
        {
            this.header = header;
        }
        
        public WebHeader(string key, string value)
        {
            header = new Header(key, value);
        }

        public static implicit operator Header(WebHeader d) => d.header;
        
        public static explicit operator WebHeader(Header b) => new WebHeader(b);

        public override string ToString() => $"{header.Key} : {header.Value}";
        
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{header.Key} : {header.Value}";
        }
    }
}