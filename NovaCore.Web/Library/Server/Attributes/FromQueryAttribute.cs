using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.ModelBinders;

namespace NovaCore.Web.Server.Attributes;

public class FromQueryAttribute : PrefixAttribute
{
    public FromQueryAttribute(string prefix)
        : base(prefix)
    {
    }
        
    public override T Get<T>(IHttpContext context, IModelBinder binder)
    {
        return binder.Get<T>(context.Request.QueryString, Prefix);
    }
}