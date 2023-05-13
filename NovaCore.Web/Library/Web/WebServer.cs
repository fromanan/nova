using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NovaCore.Common.Utilities;
using NovaCore.Files;
using NovaCore.Web.Extensions;
using NovaCore.Web.Server;
using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.Listeners;
using NovaCore.Web.Server.RequestProviders;
using NovaCore.Web.Server.Responses;
using HttpResponse = NovaCore.Web.Server.Responses.HttpResponse;
using Logger = NovaCore.Common.Logging.Logger;
using LoggingChannel = NovaCore.Common.Logging.LoggingChannel;

namespace NovaCore.Web;

public abstract class WebServer : IDisposable
{
    #region Data Members

    protected HttpServer HttpServer;
        
    protected NovaTcpListener TcpListener;
        
    public readonly int Port;
        
    public readonly Logger Logger;

    public readonly StringWriter ServerLog = new();
        
    public readonly LoggingChannel ServerLoggingChannel;

    #endregion

    #region Attributes

    public bool Serving => HttpServer.Serving;

    #endregion

    #region Constructor

    protected WebServer(int port, Logger logger = null)
    {
        Port = port;
        Logger = logger ?? new Logger();

        // Startup Logging Service
        ServerLoggingChannel = new LoggingChannel(ServerLog);
        ServerLoggingChannel.SubscribeLogger(Logger);

        // Log Warning if Port Overlaps
        if (!Webtools.IsPortAvailable(port))
            Logger.LogWarning(ServerMessages.TcpOverlap);

        // Start the HttpServer
        StartServer();
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        HttpServer.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Abstract Methods

    public abstract Task Handler(IHttpContext context, Func<Task> next);

    #endregion

    #region Public Methods

    public void StartServer()
    {
        // Create Server Instance
        HttpServer = new HttpServer(new HttpRequestProvider(), Logger);

        // Begin Listening at Port: [port]
        TcpListener = new NovaTcpListener(IPAddress.Loopback, Port)
        {
            Server = { LingerState = new LingerOption(false, 0) }
        };

        // Allow a socket to use an occupied port
        TcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        // Accept requests from the listener
        HttpServer.Use(new TcpListenerAdapter(TcpListener));

        // This is how the server will handle requests
        HttpServer.Use(Handler);

        // Begin Listening on Server
        HttpServer.Start();
    }

    public void Close()
    {
        Logger.LogInfo(ServerMessages.Closing);
        HttpServer.CloseServer();
        //TcpListener.Server.Shutdown(SocketShutdown.Both);
        //TcpListener.Server.Disconnect(true);
    }

    public void CloseAllConnections()
    {
        Logger.LogWarning(ServerMessages.CloseWarning);
        HttpServer.CloseAllConnections();
    }

    public async Task AwaitCompletion()
    {
        await Tasks.WaitUntil(() => !Serving);
    }

    public void WaitForClose()
    {
        HttpServer.WaitForClose();
    }

    public static HttpResponse PostResponse(HttpResponseCode code, string response = null,
        bool keepConnectionAlive = false)
    {
        return new HttpResponse(code, response ?? string.Empty, keepConnectionAlive);
    }
    
    public static bool HasQuery(IHttpContext context, string key)
    {
        return context.Request.QueryString.HasValue(key);
    }
    
    public static string GetQuery(IHttpContext context, string key)
    {
        return context.Request.QueryString.GetByName(key);
    }

    public static bool TryGetQuery(IHttpContext context, string key, out string value)
    {
        return context.Request.QueryString.TryGetByName(key, out value);
    }

    public void SaveServerLogs(bool showFile = false, bool openFile = false)
    {
        string filepath = FileSystem.SaveToFile(ServerLog.ToString(),
            $"{FileSystem.TimestampFilename("server")}.log",
            Paths.Downloads, "Server Logs");
            
        if (showFile)
            FileSystem.ShowFileLocation(filepath);

        if (openFile)
            FileSystem.OpenWithDefaultProgram(filepath);
    }

    #endregion

    #region Protected Methods

    protected static HttpResponse Close(HttpResponseCode code, string response = null)
    {
        return PostResponse(code, response);
    }

    protected static void Die(IHttpContext context, string errorMessage, HttpResponseCode code)
    {
        context.Response = PostResponse(code, $"<b>{errorMessage}</b>");
        throw new Exception(errorMessage);
    }

    #endregion
}