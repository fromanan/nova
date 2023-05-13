using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Headers;
using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.Responses;

namespace NovaCore.Web.Server.Handlers.Compression;

public class CompressedResponse : IHttpResponse
{
    private readonly MemoryStream _memoryStream;
    
    public HttpResponseCode ResponseCode { get; }
        
    public IHttpHeaders Headers { get; }
        
    public bool CloseConnection { get; }

    public CompressedResponse(IHttpResponse child, MemoryStream memoryStream, string encoding)
    {
        _memoryStream = memoryStream;
        ResponseCode = child.ResponseCode;
        CloseConnection = child.CloseConnection;
        Headers = new ListHttpHeaders(
        child.Headers.Where(h =>
            {
                return !h.Key.Equals("content-length", StringComparison.InvariantCultureIgnoreCase);
            })
            .Concat(new[]
            {
                new StringPair("content-length", memoryStream.Length.ToString()),
                new StringPair("content-encoding", encoding),
            })
            .ToList());
    }

    public static async Task<IHttpResponse> Create(string name, IHttpResponse child, Func<Stream, Stream> streamFactory)
    {
        MemoryStream memoryStream = new();
        await using (Stream deflateStream = streamFactory(memoryStream))
        await using (StreamWriter deflateWriter = new(deflateStream))
        {
            await child.WriteBody(deflateWriter).ContextIndependent();
            await deflateWriter.FlushAsync().ContextIndependent();
        }

        return new CompressedResponse(child, memoryStream, name);
    }

    public static Task<IHttpResponse> CreateDeflate(IHttpResponse child)
    {
        return Create("deflate", child, s =>
        {
            return new DeflateStream(s, CompressionMode.Compress, true);
        });
    }

    public static Task<IHttpResponse> CreateGZip(IHttpResponse child)
    {
        return Create("gzip", child, s =>
        {
            return new GZipStream(s, CompressionMode.Compress, true);
        });
    }

    public async Task WriteBody(StreamWriter writer)
    {
        _memoryStream.Position = 0;
        await writer.FlushAsync().ContextIndependent();
        await _memoryStream.CopyToAsync(writer.BaseStream).ContextIndependent();
    }
}