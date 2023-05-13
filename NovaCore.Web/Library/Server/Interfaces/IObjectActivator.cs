using System;

namespace NovaCore.Web.Server.Interfaces;

public interface IObjectActivator
{
    object Activate(Type type, Func<string, Type, object> argumentGetter = null);
}