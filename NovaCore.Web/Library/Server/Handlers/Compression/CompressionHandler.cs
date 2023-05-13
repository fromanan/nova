using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Extensions;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Handlers.Compression;

/// <summary>
/// An <see cref="IHttpRequestHandler"/>
/// 
/// That lets the following <see cref="IHttpRequestHandler"/>s in the chain to run
/// and afterwards tries to compress the returned response by the "Accept-Encoding" header that
/// given from the client.
/// 
/// The compressors given in the constructor are preferred by the order that they are given.
/// </summary>
public record CompressionHandler(IEnumerable<ICompressor> Compressors) : IHttpRequestHandler
{
    public async Task Handle(IHttpContext context, Func<Task> next)
    {
        await next().ContextIndependent();

        if (context.Response is null || context.SupportedEncodings is not { } encodings)
            return;

        if (Compressors.GetSupportedCompressor(encodings) is not { } compressor)
            return;

        context.Response = await compressor.Compress(context.Response).ContextIndependent();
    }
}