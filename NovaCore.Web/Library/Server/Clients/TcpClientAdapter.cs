using System.IO;
using System.Net;
using System.Net.Sockets;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Clients;

public readonly struct TcpClientAdapter : IClient
{
    private readonly TcpClient _client;
    
    private readonly NetworkStream _stream;
    
    public Stream Stream => _stream;

    public bool Connected => _client.Connected;
    
    public EndPoint RemoteEndPoint => _client.Client.RemoteEndPoint;

    public TcpClientAdapter(TcpClient client)
    {
        _client = client;
        _stream = _client.GetStream();
    }

    public void Close()
    {
        _client.Close();
    }
}