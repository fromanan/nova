using System.Collections.Generic;

namespace NovaCore.Web;

public record WebQuery(string Key, string Value)
{
    public static implicit operator StringPair(WebQuery webQuery)
    {
        return new StringPair(webQuery.Key, webQuery.Value);
    }

    public static explicit operator WebQuery(StringPair keyValuePair)
    {
        return new WebQuery(keyValuePair.Key, keyValuePair.Value);
    }
}