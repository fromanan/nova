using System;

namespace NovaCore.Web.Server.Handlers;

internal sealed class ControllerMethod
{
    public ControllerMethod(Type controllerType, HttpMethods method)
    {
        ControllerType = controllerType;
        Method = method;
    }

    public Type ControllerType { get; }

    public HttpMethods Method { get; }

    private bool Equals(ControllerMethod other)
    {
        return ControllerType == other.ControllerType && Method == other.Method;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj is ControllerMethod method && Equals(method);
    }

    public override int GetHashCode()
    {
        unchecked { return (ControllerType.GetHashCode() * 397) ^ (int)Method; }
    }
}