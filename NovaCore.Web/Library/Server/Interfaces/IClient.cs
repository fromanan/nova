using System.IO;
using System.Net;

namespace NovaCore.Web.Server.Interfaces;

public interface IClient
{
    Stream Stream { get; }

    bool Connected { get; }

    void Close();

    EndPoint RemoteEndPoint { get; }
}