using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.ModelBinders;

namespace NovaCore.Web.Server.Attributes;

public class FromBodyAttribute : PrefixAttribute
{
    public FromBodyAttribute(string prefix = null)
        : base(prefix)
    {
    }

    public override T Get<T>(IHttpContext context, IModelBinder binder)
    {
        return binder.Get<T>(context.Request.Post.Raw, Prefix);
    }
}