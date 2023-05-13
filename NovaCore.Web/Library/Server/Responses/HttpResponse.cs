using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Responses;

public sealed class HttpResponse : IHttpResponse
{
    #region Data Members

    private readonly Stream _headerStream = new MemoryStream();

    #endregion

    #region Attributes

    private Stream ContentStream { get; set; }
    
    public HttpResponseCode ResponseCode { get; }
    
    public IHttpHeaders Headers { get; }

    public bool CloseConnection { get; }

    #endregion

    #region Constructor
    
    public HttpResponse(HttpResponseCode code, string contentType, Stream contentStream, bool keepAliveConnection,
        IEnumerable<StringPair> headers)
    {
        ContentStream = contentStream;
        CloseConnection = !keepAliveConnection;
        ResponseCode = code;
        Headers = Webtools.GenerateHeaders(contentType, keepAliveConnection, ContentStream.Length, headers);
    }

    public HttpResponse(HttpResponseCode code, string content, bool closeConnection)
        : this(code, Webtools.DEFAULT_CONTENT_TYPE, StringToStream(content), closeConnection)
    {
    }

    public HttpResponse(HttpResponseCode code, string content, IEnumerable<StringPair> headers,
        bool closeConnection)
        : this(code, Webtools.DEFAULT_CONTENT_TYPE, StringToStream(content), closeConnection, headers)
    {
    }

    public HttpResponse(string contentType, Stream contentStream, bool closeConnection)
        : this(HttpResponseCode.Ok, contentType, contentStream, closeConnection)
    {
    }

    public HttpResponse(HttpResponseCode code, string contentType, Stream contentStream, bool keepAliveConnection) :
        this(code, contentType, contentStream, keepAliveConnection, Enumerable.Empty<StringPair>())
    {
    }

    public HttpResponse(HttpResponseCode code, byte[] contentStream, bool keepAliveConnection)
        : this(code, Webtools.DEFAULT_CONTENT_TYPE, new MemoryStream(contentStream), keepAliveConnection)
    {
    }

    #endregion

    #region Public Methods

    public async Task WriteBody(StreamWriter writer)
    {
        ContentStream.Position = 0;
        await ContentStream.CopyToAsync(writer.BaseStream).ContextIndependent();
    }

    public async Task WriteHeaders(StreamWriter writer)
    {
        _headerStream.Position = 0;
        await _headerStream.CopyToAsync(writer.BaseStream).ContextIndependent();
    }

    #endregion
    
    #region Static Methods

    public static HttpResponse CreateWithMessage(HttpResponseCode code, string message, bool keepAliveConnection,
        string body = "")
    {
        return new HttpResponse(code, string.Format(ServerMessages.BasicResponse, message, body), keepAliveConnection);
    }
    
    private static MemoryStream StringToStream(string content)
    {
        MemoryStream stream = new();
        StreamWriter writer = new(stream);
        writer.Write(content);
        writer.Flush();
        return stream;
    }

    #endregion
}