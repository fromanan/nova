using System;
using NovaCore.Web.Server.Headers;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server;

internal record EmptyHttpPost : IHttpPost
{
    private static readonly byte[] EmptyBytes = Array.Empty<byte>();

    public static readonly IHttpPost Empty = new EmptyHttpPost();

    private EmptyHttpPost() { }

    #region IHttpPost implementation
    
    public byte[] Raw => EmptyBytes;

    public IHttpHeaders Parsed => EmptyHttpHeaders.Empty;
    
    #endregion
}