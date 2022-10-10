using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NovaCore.Common.Utilities;
using NovaCore.Files;
using uhttpsharp;
using uhttpsharp.Listeners;
using uhttpsharp.RequestProviders;
using HttpResponse = uhttpsharp.HttpResponse;
using Logger = NovaCore.Common.Logging.Logger;
using LoggingChannel = NovaCore.Common.Logging.LoggingChannel;

namespace NovaCore.Web
{
    public abstract class WebServer : IDisposable
    {
        protected HttpServer HttpServer;
        protected NovaTcpListener TcpListener;
        public readonly int Port;
        public readonly Logger Logger;

        public readonly StringWriter ServerLog = new();
        public readonly Common.Logging.LoggingChannel ServerLoggingChannel;

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
                Logger.LogWarning("Possible TCP port overlap with web server");
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

        public void Close()
        {
            Logger.LogInfo("Closing server...");
            HttpServer.CloseServer();
            //TcpListener.Server.Shutdown(SocketShutdown.Both);
            //TcpListener.Server.Disconnect(true);
        }

        public void CloseAllConnections()
        {
            Logger.LogWarning("User requested close of all connections may lead to errors");
            HttpServer.CloseAllConnections();
        }

        public bool Serving => HttpServer.Serving;

        public static byte[] GetBytes(string s) => Encoding.UTF8.GetBytes(s);

        public abstract Task Handler(IHttpContext context, Func<Task> next);

        public async Task AwaitCompletion()
        {
            await Tasks.WaitUntil(() => !Serving);
        }

        public void WaitForClose()
        {
            HttpServer.WaitForClose();
        }

        public static HttpResponse PostResponse(HttpResponseCode code, string response = null, bool keepConnectionAlive = false)
        {
            return new HttpResponse(code, response ?? "", keepConnectionAlive);
            //return new HttpResponse(code, response, true);
        }

        protected static HttpResponse Close(HttpResponseCode code = HttpResponseCode.Ok, string response = null)
        {
            return PostResponse(code, response);
            //return new HttpResponse(HttpResponseCode.Ok, GetBytes(response), false);
        }

        protected static void Die(IHttpContext context, string errorMessage, HttpResponseCode code)
        {
            context.Response = PostResponse(code, $"<b>{errorMessage}</b>");
            throw new Exception(errorMessage);
        }

        public static bool GetQuery(IHttpContext context, string key, out string value)
        {
            return context.Request.QueryString.TryGetByName(key, out value);
        }

        public void SaveServerLogs(bool showFile = false, bool openFile = false)
        {
            string filepath = FileSystem.SaveToFile(ServerLog.ToString(),
                $"{FileSystem.TimestampFilename("server")}.log",
                FileSystem.Paths.Downloads, "Server Logs");
            
            if (showFile)
            {
                FileSystem.ShowFileLocation(filepath);
            }

            if (openFile)
            {
                FileSystem.OpenWithDefaultProgram(filepath);
            }
        }
    }
}