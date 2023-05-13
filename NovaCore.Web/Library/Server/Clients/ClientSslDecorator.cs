using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Clients
{
    public class ClientSslDecorator : IClient
    {
        private readonly IClient _child;
        
        private readonly X509Certificate _certificate;
        
        private readonly SslStream _sslStream;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        
        public Stream Stream => _sslStream;

        public bool Connected => _child.Connected;
        
        public EndPoint RemoteEndPoint => _child.RemoteEndPoint;

        public ClientSslDecorator(IClient child, X509Certificate certificate)
        {
            _child = child;
            _certificate = certificate;
            _sslStream = new SslStream(_child.Stream);
        }

        public async Task AuthenticateAsServer()
        {
            SslServerAuthenticationOptions options = new()
            {
                ServerCertificate = _certificate,
                ClientCertificateRequired = false,
                EnabledSslProtocols = SslProtocols.Tls12,
                CertificateRevocationCheckMode = X509RevocationMode.Online,
                EncryptionPolicy = EncryptionPolicy.RequireEncryption
            };
            
            _cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));

            await _sslStream.AuthenticateAsServerAsync(options, _cancellationTokenSource.Token);
            
            if (_cancellationTokenSource.IsCancellationRequested)
                throw new TimeoutException("SSL Authentication Timeout");
        }

        public void Close()
        {
            _child.Close();
        }
    }
}