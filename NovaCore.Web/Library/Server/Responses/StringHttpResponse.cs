using System.IO;
using System.Text;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Headers;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Responses;

public sealed class StringHttpResponse : HttpResponseBase
{
    private readonly string _body;

    public StringHttpResponse(string body, HttpResponseCode code, IHttpHeaders headers)
        : base(code, headers)
    {
        _body = body;
    }

    public static IHttpResponse Create(string body, HttpResponseCode code = HttpResponseCode.Ok,
        string contentType = Webtools.DEFAULT_CONTENT_TYPE, bool keepAlive = true, IHttpHeaders headers = null)
    {
        // TODO : Add Overload
        headers ??= EmptyHttpHeaders.Empty;

        long contentLength = Encoding.UTF8.GetByteCount(body);

        ListHttpHeaders children = Webtools.GenerateHeaders(contentType, keepAlive, contentLength);
        
        return new StringHttpResponse(body, code, new CompositeHttpHeaders(children, headers));
    }

    public override async Task WriteBody(StreamWriter writer)
    {
        await writer.WriteAsync(_body).ContextIndependent();
    }
}