using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NovaCore.Common;
using NovaCore.Files;
using NovaCore.Logging;
using NovaCore.Utilities;
using uhttpsharp;
using uhttpsharp.Listeners;
using uhttpsharp.RequestProviders;
using HttpResponse = uhttpsharp.HttpResponse;

namespace NovaCore.Web
{
    public abstract class WebServer : IDisposable
    {
        protected HttpServer HttpServer;
        protected NovaTcpListener TcpListener;
        public readonly int Port;
        public readonly Logger Logger;

        public readonly StringWriter ServerLog = new();
        public readonly LoggingChannel ServerLoggingChannel;

        protected WebServer(int port, Logger logger = null)
        {
            Port = port;
            Logger = logger ?? new Logger();

            // Startup Logging Service
            ServerLoggingChannel = new LoggingChannel(ServerLog);
            ServerLoggingChannel.SubscribeLogger(Logger);

            // Log Warning if Port Overlaps
            if (!WebUtils.IsPortAvailable(port))
            {
                Logger.LogWarning("Possible TCP port overlap with authentication server");
            }

            // Start the HttpServer
            StartServer();
        }

        // Implementation of IDisposable
        public void Dispose()
        {
            HttpServer.Dispose();
        }

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

        public void StopServer() => HttpServer.Dispose();

        public void Close()
        {
            HttpServer.CloseServer();
            //TcpListener.Server.Shutdown(SocketShutdown.Both);
            //TcpListener.Server.Disconnect(true);
        }

        public void CloseAllConnections() => HttpServer.CloseAllConnections();

        public bool Serving => HttpServer.Serving;

        public static byte[] GetBytes(string s) => Encoding.UTF8.GetBytes(s);

        public abstract Task Handler(IHttpContext context, Func<Task> next);

        public async Task AwaitCompletion()
        {
            await Tasks.WaitUntil(() => !Serving);
        }

        protected static HttpResponse Close(string response)
        {
            return new HttpResponse(HttpResponseCode.Ok, GetBytes(response), false);
        }

        protected static void Die(IHttpContext context, string errorMessage)
        {
            context.Response = Close($"<b>{errorMessage}</b>");
            throw new Exception(errorMessage);
        }

        public static bool GetQuery(IHttpContext context, string key, out string value)
        {
            return context.Request.QueryString.TryGetByName(key, out value);
        }

        public void SaveServerLogs()
        {
            string filepath = FileSystem.SaveToFile(ServerLog.ToString(),
                $"{FileSystem.TimestampFilename("server")}.log",
                FileSystem.Paths.Downloads, "Server Logs");
            FileSystem.ShowFileLocation(filepath);
        }
    }
}