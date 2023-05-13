using System;
using Parameter = System.Collections.Generic.KeyValuePair<string, string>;

namespace NovaCore.Web;

public record WebParameter : IFormattable
{
    private readonly Parameter _parameter;

    public string Key => _parameter.Key;

    public string Value => _parameter.Value;

    public WebParameter(Parameter parameter)
    {
        _parameter = parameter;
    }
        
    public WebParameter(string key, string value)
    {
        _parameter = new Parameter(key, value);
    }
    
    public string ParameterString => $"{_parameter.Key} : {_parameter.Value}";

    public static implicit operator Parameter(WebParameter d) => d._parameter;
        
    public static explicit operator WebParameter(Parameter b) => new(b);

    public override string ToString()
    {
        return ParameterString;
    }
        
    public string ToString(string format, IFormatProvider formatProvider)
    {
        return ParameterString;
    }
}