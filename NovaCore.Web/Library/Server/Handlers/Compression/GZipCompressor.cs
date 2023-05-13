using System.Threading.Tasks;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Handlers.Compression;

public class GZipCompressor : ICompressor
{
    public static readonly ICompressor Default = new GZipCompressor();

    public string Name => "gzip";
        
    public Task<IHttpResponse> Compress(IHttpResponse response)
    {
        return CompressedResponse.CreateGZip(response);
    }
}