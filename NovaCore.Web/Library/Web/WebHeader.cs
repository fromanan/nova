using System;
using Header = System.Collections.Generic.KeyValuePair<string, string>;

namespace NovaCore.Web;

public record WebHeader : IFormattable
{
    private readonly Header _header;

    public string Key => _header.Key;

    public string Value => _header.Value;

    public WebHeader(Header header)
    {
        _header = header;
    }
        
    public WebHeader(string key, string value)
    {
        _header = new Header(key, value);
    }

    public string HeaderString => $"{_header.Key} : {_header.Value}";

    public static implicit operator Header(WebHeader d) => d._header;
        
    public static explicit operator WebHeader(Header b) => new(b);

    public override string ToString()
    {
        return HeaderString;
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return HeaderString;
    }
}