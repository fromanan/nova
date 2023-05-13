using System;
using System.Collections.Generic;

namespace NovaCore.Web.Server;

public sealed record HttpRequestParameters
{
    private readonly string[] _params;

    public IList<string> Params => _params;
    
    public HttpRequestParameters(Uri uri)
    {
        _params = uri.OriginalString.Split('/', StringSplitOptions.RemoveEmptyEntries);
    }
}