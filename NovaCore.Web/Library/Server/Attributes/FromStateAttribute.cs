using System;
using System.Collections.Generic;
using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.ModelBinders;

namespace NovaCore.Web.Server.Attributes;

public class FromStateAttribute : Attribute, IModelBinding
{
    private readonly string _propertyName;
    
    public FromStateAttribute(string propertyName)
    {
        _propertyName = propertyName;
    }
    
    public T Get<T>(IHttpContext context, IModelBinder binder)
    {
        // Expando object
        if (context.State is IDictionary<string, object> state && state.TryGetValue(_propertyName, out object real) &&
            real is T value)
        {
            return value;
        }

        return default;
    }
}