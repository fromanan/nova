using System;
using System.Collections.Generic;
using System.Linq;
using NovaCore.Common.Extensions;

namespace NovaCore.Web.Server.Interfaces;

public interface IHttpHeaders : IEnumerable<StringPair>
{
    string GetByName(string name);

    bool TryGetByName(string name, out string value);

    bool HasValue(string name);

    bool HasValue(string name, string value, StringComparison comparison = Webtools.IGNORE_CASE);
    
    public string this[string key] => GetByName(key);

    #region Extension Methods

    public bool KeepAliveConnection()
    {
        return HasValue("Connection", "Keep-Alive");
    }

    public bool TryGetByName<T>(string name, out T value)
    {
        if (TryGetByName(name, out string stringValue))
        {
            value = (T)Convert.ChangeType(stringValue, typeof(T));
            return true;
        }

        value = default;
        return false;
    }

    public T GetByName<T>(string name)
    {
        TryGetByName(name, out T value);
        return value;
    }

    public T GetByNameOrDefault<T>(string name, T defaultValue)
    {
        return TryGetByName(name, out T value) ? value : defaultValue;
    }

    public string ToUriData()
    {
        return this.Select(Webtools.FormatPair).Merge("&");
    }

    #endregion
}