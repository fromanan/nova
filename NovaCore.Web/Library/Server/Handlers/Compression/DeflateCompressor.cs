using System.Threading.Tasks;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Handlers.Compression;

public class DeflateCompressor : ICompressor
{
    public static readonly ICompressor Default = new DeflateCompressor();

    public string Name => "deflate";
        
    public Task<IHttpResponse> Compress(IHttpResponse response)
    {
        return CompressedResponse.CreateDeflate(response);
    }
}