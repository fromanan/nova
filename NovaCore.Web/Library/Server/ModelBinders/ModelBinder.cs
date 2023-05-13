using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.ModelBinders;

public record ModelBinder(IObjectActivator Activator) : IModelBinder
{
    public T Get<T>(byte[] raw, string prefix)
    {
        throw new NotSupportedException();
    }
    
    public T Get<T>(IHttpHeaders headers, string prefix)
    {
        return (T)Get(typeof(T), headers, prefix);
    }

    public T Get<T>(IHttpHeaders headers)
    {
        T returnValue = Activator.Activate<T>();

        foreach (PropertyInfo prop in GetProperties(returnValue))
        {
            object value;
            
            if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string))
            {
                if (!headers.HasValue(prop.Name))
                    continue;
                value = Convert.ChangeType(headers.GetByName(prop.Name), prop.PropertyType);
            }
            else
            {
                value = Get(prop.PropertyType, headers, prop.Name);
            }
            
            prop.SetValue(returnValue, value);
        }

        return returnValue;
    }

    private object Get(Type type, IHttpHeaders headers, string prefix)
    {
        if (type.IsPrimitive || type == typeof(string))
        {
            string value = headers.GetByNameOrDefault<string>(prefix, null);
            return value is not null ? Convert.ChangeType(value, type) : null;
        }

        object returnValue = Activator.Activate(type);

        List<PropertyInfo> setValues = GetProperties(returnValue)
                .Where(p => headers.HasValue($"{prefix}[{p.Name}]"))
                .ToList();

        if (setValues.Count == 0)
            return null;

        foreach (PropertyInfo prop in setValues.Where(prop => headers.HasValue($"{prefix}[{prop.Name}]")))
        {
            object value;

            if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string))
            {
                value = Convert.ChangeType(headers.GetByName($"{prefix}[{prop.Name}]"), prop.PropertyType);
            }
            else
            {
                value = Get(prop.PropertyType, headers, $"{prefix}[{prop.Name}]");
            }

            prop.SetValue(returnValue, value);
        }

        return returnValue;
    }

    private static IEnumerable<PropertyInfo> GetProperties<T>(T returnValue)
    {
        return returnValue.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }
}