using System;
using System.Text;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Headers;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server;

internal class HttpPost : IHttpPost
{
    private readonly int _readBytes;

    private readonly Lazy<IHttpHeaders> _parsed;
    
    public HttpPost(byte[] raw, int readBytes)
    {
        Raw = raw;
        _readBytes = readBytes;
        _parsed = new Lazy<IHttpHeaders>(Parse);
    }
    
    public static async Task<IHttpPost> Create(IStreamReader reader, int postContentLength)
    {
        return new HttpPost(await reader.ReadBytes(postContentLength).ContextIndependent(), postContentLength);
    }

    private IHttpHeaders Parse()
    {
        return new QueryStringHttpHeaders(Encoding.UTF8.GetString(Raw, 0, _readBytes));
    }

    #region IHttpPost implementation
    
    public byte[] Raw { get; }

    public IHttpHeaders Parsed => _parsed.Value;
    
    #endregion
}