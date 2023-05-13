using System.IO;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Responses;

public sealed class EmptyHttpResponse : HttpResponseBase
{
    public EmptyHttpResponse(HttpResponseCode code, IHttpHeaders headers)
        : base(code, headers)
    {
    }

    public static IHttpResponse Create(HttpResponseCode code = HttpResponseCode.Ok, bool keepAlive = true)
    {
        return new EmptyHttpResponse(code, Webtools.GenerateHeaders("text/html", keepAlive, 0));
    }

    public override Task WriteBody(StreamWriter writer)
    {
        return Task.Factory.GetCompleted();
    }
}
