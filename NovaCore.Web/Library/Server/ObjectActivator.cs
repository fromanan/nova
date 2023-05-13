using System;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server;

public class ObjectActivator : IObjectActivator
{
    public object Activate(Type type, Func<string, Type, object> argumentGetter)
    {
        return Activator.CreateInstance(type);
    }
}

public static class ObjectActivatorExtensions
{
    public static T Activate<T>(this IObjectActivator activator, Func<string, Type, object> argumentGetter = null)
    {
        return (T)activator.Activate(typeof(T), argumentGetter);
    }
}