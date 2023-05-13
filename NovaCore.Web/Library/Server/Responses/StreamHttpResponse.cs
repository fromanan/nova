using System.IO;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Responses;

public sealed class StreamHttpResponse : HttpResponseBase
{
    private readonly Stream _body;
        
    public StreamHttpResponse(Stream body, HttpResponseCode code, IHttpHeaders headers) : base(code, headers)
    {
        _body = body;
    }

    public static IHttpResponse Create(Stream body, HttpResponseCode code = HttpResponseCode.Ok,
        string contentType = Webtools.DEFAULT_CONTENT_TYPE, bool keepAlive = true)
    {
        return new StreamHttpResponse(body, code, Webtools.GenerateHeaders(contentType, keepAlive, body.Length));
    }

    public override async Task WriteBody(StreamWriter writer)
    {
        await writer.FlushAsync().ContextIndependent();
        await _body.CopyToAsync(writer.BaseStream).ContextIndependent();
        await writer.BaseStream.FlushAsync().ContextIndependent();
    }
}