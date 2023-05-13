using NovaCore.Web.Server.ModelBinders;

namespace NovaCore.Web.Server.Interfaces;

internal interface IModelBinding
{
    T Get<T>(IHttpContext context, IModelBinder binder);
}

