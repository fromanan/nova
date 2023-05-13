using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NovaCore.Web.Server.Headers;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.ModelBinders;

public class JsonModelBinder : IModelBinder
{
    private readonly JsonSerializer _serializer;

    public JsonModelBinder(JsonSerializer serializer)
    {
        _serializer = serializer;
    }

    public JsonModelBinder() :
        this(JsonSerializer.CreateDefault())
    {
    }
        
    public T Get<T>(byte[] raw, string prefix)
    {
        string rawDecoded = Encoding.UTF8.GetString(raw);

        if (raw.Length == 0)
            return default;

        if (prefix is null && typeof(T) == typeof(string))
            return (T)(object)rawDecoded;

        JToken jToken = JToken.Parse(rawDecoded);

        if (prefix is not null)
            jToken = jToken.SelectToken(prefix);

        return jToken is not null ? jToken.ToObject<T>(_serializer) : default;
    }
        
    public T Get<T>(IHttpHeaders headers)
    {
        throw new NotSupportedException();
    }
        
    public T Get<T>(IHttpHeaders headers, string prefix)
    {
        throw new NotSupportedException();
    }
}