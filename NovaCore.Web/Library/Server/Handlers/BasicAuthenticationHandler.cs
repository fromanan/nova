using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Headers;
using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.Responses;

namespace NovaCore.Web.Server.Handlers;

public readonly struct BasicAuthenticationHandler : IHttpRequestHandler
{
    private const string _BASIC_PREFIX = "Basic ";
        
    private static readonly int BasicPrefixLength = _BASIC_PREFIX.Length;

    private readonly string _username;
        
    private readonly string _password;
        
    private readonly string _authenticationKey;
        
    private readonly ListHttpHeaders _headers;

    public BasicAuthenticationHandler(string realm, string username, string password)
    {
        _username = username;
        _password = password;
        _authenticationKey = $"Authenticated.{realm}";
        _headers = new ListHttpHeaders(new List<StringPair>
        {
            new("WWW-Authenticate", $@"Basic realm=""{realm}""")
        });
    }

    public Task Handle(IHttpContext context, Func<Task> next)
    {
        IDictionary<string, dynamic> session = context.State.Session;

        if (session.TryGetValue(_authenticationKey, out dynamic ipAddress) && ipAddress == context.RemoteEndPoint)
            return next.Invoke();
            
        if (TryAuthenticate(context, session))
            return next.Invoke();

        context.Response = StringHttpResponse.Create("Not Authenticated", HttpResponseCode.Unauthorized,
            headers: _headers);

        return Task.Factory.GetCompleted();

    }

    private bool TryAuthenticate(IHttpContext context, IDictionary<string, dynamic> session)
    {
        if (!context.Request.Headers.TryGetByName("Authorization", out string credentials))
            return false;
            
        if (!TryAuthenticate(credentials))
            return false;
            
        session[_authenticationKey] = context.RemoteEndPoint;
        return true;
    }

    private bool TryAuthenticate(string credentials)
    {
        if (!credentials.StartsWith(_BASIC_PREFIX))
            return false;

        string basicCredentials = credentials[BasicPrefixLength..];

        string userInfo = Encoding.UTF8.GetString(Convert.FromBase64String(basicCredentials));
        
        if (userInfo.Contains(':'))
            return false;

        (string username, string password) = userInfo.SplitAt(':');

        return username == _username && password == _password;
    }
}