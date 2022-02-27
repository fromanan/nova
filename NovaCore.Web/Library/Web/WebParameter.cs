using System;
using Parameter = System.Collections.Generic.KeyValuePair<string, string>;

namespace NovaCore.Web
{
    public readonly struct WebParameter : IFormattable
    {
        private readonly Parameter parameter;

        public string Key => parameter.Key;

        public string Value => parameter.Value;

        public WebParameter(Parameter parameter)
        {
            this.parameter = parameter;
        }
        
        public WebParameter(string key, string value)
        {
            parameter = new Parameter(key, value);
        }

        public static implicit operator Parameter(WebParameter d) => d.parameter;
        
        public static explicit operator WebParameter(Parameter b) => new WebParameter(b);

        public override string ToString() => $"{parameter.Key} : {parameter.Value}";
        
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{parameter.Key} : {parameter.Value}";
        }
    }
}