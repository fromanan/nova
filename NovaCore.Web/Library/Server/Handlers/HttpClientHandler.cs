using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Extensions;
using NovaCore.Web.Resources;
using NovaCore.Web.Server.Clients;
using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.RequestProviders;
using NovaCore.Web.Server.Streams;
using Logger = NovaCore.Common.Logging.Logger;

namespace NovaCore.Web.Server.Handlers;

internal sealed class HttpClientHandler : IDisposable
{
    private readonly Func<IHttpContext, Task> _requestHandler;
    
    private readonly IHttpRequestProvider _requestProvider;
    
    private readonly EndPoint _remoteEndPoint;
    
    private Stream _stream;
    
    private readonly Logger _logger;

    private int _bufferSize;
    
    public IClient Client { get; }
    
    public DateTime LastOperationTime { get; private set; }

    public HttpClientHandler(IClient client, Func<IHttpContext, Task> requestHandler,
        IHttpRequestProvider requestProvider, Logger logger)
    {
        _remoteEndPoint = client.RemoteEndPoint;
        Client = client;

        _requestHandler = requestHandler;
        _requestProvider = requestProvider;
        _logger = logger;

        // Logger.InfoFormat(Text.GotClient, _remoteEndPoint);
        _logger.Log(string.Format(Text.GotClient, _remoteEndPoint));

        Task.Factory.StartNew(Process);

        UpdateLastOperationTime();
    }

    private async Task InitializeStream(int bufferSize = 8096)
    {
        if (Client is ClientSslDecorator client)
            await client.AuthenticateAsServer().ContextIndependent();

        _bufferSize = bufferSize;

        _stream = new BufferedStream(Client.Stream, _bufferSize);
    }

    private async Task HandleRequest(IHttpRequest request, NotFlushingStream limitedStream)
    {
        UpdateLastOperationTime();

        HttpContext context = new(request, Client.RemoteEndPoint);

        // Logger.InfoFormat(Text.GotRequest, Client.RemoteEndPoint, request.Uri);
        _logger.Log(string.Format(Text.GotRequest, Client.RemoteEndPoint, request.Uri));

        await _requestHandler(context).ContextIndependent();

        if (context.Response is not null)
        {
            await using StreamWriter streamWriter = new(limitedStream)
            {
                AutoFlush = false,
                NewLine = Environment.NewLine
            };
            await WriteResponse(context, streamWriter).ContextIndependent();
            await limitedStream.ExplicitFlushAsync().ContextIndependent();
            if (!request.KeepAlive() || context.Response.CloseConnection)
                CloseClient();
        }

        UpdateLastOperationTime();
    }

    private async Task ServeClient()
    {
        // TODO : Configuration.
        await using LimitedStream limitedStream = new(_stream);
        await using NotFlushingStream notFlushingStream = new(limitedStream);
        using MyStreamReader streamReader = new(notFlushingStream);

        if (await _requestProvider.Provide(streamReader).ContextIndependent() is not { } request)
        {
            CloseClient();
            return;
        }

        await HandleRequest(request, notFlushingStream);
    }

    private async void Process()
    {
        try
        {
            await InitializeStream();

            while (Client.Connected)
            {
                await ServeClient();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format(Text.ErrorServing, _remoteEndPoint));
            _logger.LogException(ex);
            CloseClient();
        }

        // Logger.InfoFormat(Text.LostClient, _remoteEndPoint);
        _logger.Log(string.Format(Text.LostClient, _remoteEndPoint));
    }

    private static async Task WriteResponse(IHttpContext context, StreamWriter writer)
    {
        IHttpResponse response = context.Response;
        IHttpRequest request = context.Request;

        // Headers
        string headerText = string.Format(Text.ClientResponseHeader, (int)response.ResponseCode, response.ResponseCode);
        await writer.WriteLineAsync(headerText).ContextIndependent();

        foreach ((string key, string value) in response.Headers)
            await writer.WriteLineAsync($"{key}: {value}").ContextIndependent();

        // Cookies
        if (context.Cookies.Touched)
            await writer.WriteAsync(context.Cookies.ToCookieData()).ContextIndependent();

        // Empty Line
        await writer.WriteLineAsync().ContextIndependent();
        await writer.FlushAsync();

        // Body
        await response.WriteBody(writer).ContextIndependent();
        await writer.FlushAsync().ContextIndependent();
    }

    public void CloseClient()
    {
        Client.Close();
    }

    private void UpdateLastOperationTime()
    {
        LastOperationTime = DateTime.Now;
    }

    public void Dispose()
    {
        _stream?.Dispose();
        _logger?.Dispose();
    }
}