using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Extensions;
using NovaCore.Web.Server.Handlers;
using NovaCore.Web.Server.Interfaces;
using Logger = NovaCore.Common.Logging.Logger;

namespace NovaCore.Web.Server;

public sealed class HttpServer : IDisposable
{
    private readonly IList<IHttpRequestHandler> _handlers = new List<IHttpRequestHandler>();
    private readonly IList<IHttpListener> _listeners = new List<IHttpListener>();
    private readonly IList<HttpClientHandler> _clientHandlers = new List<HttpClientHandler>();
    
    private readonly IHttpRequestProvider _requestProvider;

    private readonly AutoResetEvent _serverStoppedHandle = new(false);

    public readonly Logger Logger;
    
    public SafeWaitHandle ServerWaitHandle => _serverStoppedHandle.SafeWaitHandle;

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public bool Active => !_cancellationTokenSource.IsCancellationRequested;

    public bool Serving => !_clientHandlers.Any(c => c.Client.Connected);

    public HttpServer(IHttpRequestProvider requestProvider, Logger logger = null)
    {
        _requestProvider = requestProvider;
        Logger = logger ?? new Logger();
    }

    public void Use(IHttpRequestHandler handler)
    {
        _handlers.Add(handler);
    }

    public void Use(IHttpListener listener)
    {
        _listeners.Add(listener);
    }

    public void Start()
    {
        _listeners.ForEach(AddListener);
        
        // Logger.InfoFormat(ServerMessages.Started);
        Logger.LogInfo(ServerMessages.Started);
    }

    private void AddListener(IHttpListener listener)
    {
        void ListenTask() { Listen(listener); }
        Task.Factory.StartNew(ListenTask);
    }

    private async void Listen(IHttpListener listener)
    {
        Func<IHttpContext, Task> aggregatedHandler = _handlers.Aggregate();
        while (Active)
        {
            try { await CreateClientHandler(listener, aggregatedHandler).ContextIndependent(); }
            catch (Exception ex) { HandleClientException(ex); }
        }

        // Allow all connections to be closed
        await Task.Run(CloseAllConnections);

        // Alerts any awaiters for stopping that we are done
        _serverStoppedHandle.Set();

        // Logger.InfoFormat(ServerMessages.Stopped);
        Logger.LogInfo(ServerMessages.Stopped);
    }

    private async Task CreateClientHandler(IHttpListener listener, Func<IHttpContext, Task> aggregatedHandler)
    {
        IClient client = await listener.GetClient().ContextIndependent();
        _clientHandlers.Add(new HttpClientHandler(client, aggregatedHandler, _requestProvider, Logger));
    }

    private void HandleClientException(Exception ex)
    {
        // Logger.WarnException(ServerMessages.ErrorGettingClient, e);
        Logger.LogError(ServerMessages.ErrorGettingClient);
        Logger.LogException(ex);
    }

    public void Dispose()
    {
        CloseServer();
    }

    public void CloseServer()
    {
        _cancellationTokenSource.Cancel();
    }

    public void WaitForClose()
    {
        _serverStoppedHandle.WaitOne();
    }

    public void CloseAllConnections()
    {
        void CloseConnection(HttpClientHandler clientHandler)
        {
            clientHandler?.CloseClient();
        }
        
        _clientHandlers.ForEach(CloseConnection);
    }
}