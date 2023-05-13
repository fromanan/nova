using System;
using System.Collections.Generic;

namespace NovaCore.Web.Server.Handlers;

internal sealed class ControllerRoute
{
    private readonly Type _controllerType;
        
    private readonly string _propertyName;
        
    private readonly IEqualityComparer<string> _propertyNameComparer;

    public ControllerRoute(Type controllerType, string propertyName, IEqualityComparer<string> propertyNameComparer)
    {
        _controllerType = controllerType;
        _propertyName = propertyName;
        _propertyNameComparer = propertyNameComparer;
    }

    private bool Equals(ControllerRoute other)
    {
        return other is not null
               && _controllerType == other._controllerType
               && _propertyNameComparer.Equals(_propertyName, other._propertyName);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return ReferenceEquals(this, obj) || Equals(obj as ControllerRoute);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((_controllerType is not null ? _controllerType.GetHashCode() : 0) * 397) ^
                   (_propertyName is not null ? _propertyNameComparer.GetHashCode(_propertyName) : 0);
        }
    }
}