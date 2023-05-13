using System;
using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.ModelBinders;

namespace NovaCore.Web.Server.Attributes;

public abstract class PrefixAttribute : Attribute, IModelBinding
{
    public PrefixAttribute(string prefix)
    {
        Prefix = prefix;
    }

    public bool HasPrefix => !string.IsNullOrEmpty(Prefix);

    public string Prefix { get; }

    public abstract T Get<T>(IHttpContext context, IModelBinder binder);
}