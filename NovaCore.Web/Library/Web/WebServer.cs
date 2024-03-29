using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NovaCore.Utilities;
using NovaCore.Web.Interfaces;
using uhttpsharp;
using uhttpsharp.Listeners;
using uhttpsharp.RequestProviders;

namespace NovaCore.Web
{
    public abstract class WebServer : IDisposable, ICallback
    {
        protected HttpServer HttpServer;
        protected NovaTcpListener TcpListener;
        public readonly int Port;
        
        protected WebServer(int port)
        {
            Port = port;
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
            HttpServer = new HttpServer(new HttpRequestProvider());

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
            TcpListener.Server.Shutdown(SocketShutdown.Both);
            //TcpListener.Server.Disconnect(true);
        }

        public bool Active => TcpListener.Active;

        public static byte[] GetBytes(string s) => Encoding.UTF8.GetBytes(s);

        public abstract Task Handler(IHttpContext context, Func<Task> next);
        
        public async Task AwaitCompletion()
        {
            await Tasks.WaitUntil(() => !TcpListener.Pending());
        }
        
        protected static HttpResponse Close(string response)
        {
            return new HttpResponse(HttpResponseCode.Ok, GetBytes(response), false);
        }

        protected void Die(IHttpContext context, string errorMessage)
        {
            context.Response = Close($"<b>{errorMessage}</b>");
            throw new Exception(errorMessage);
        }
        
        public static bool GetQuery(IHttpContext context, string key, out string value)
        {
            return context.Request.QueryString.TryGetByName(key, out value);
        }
    }
}