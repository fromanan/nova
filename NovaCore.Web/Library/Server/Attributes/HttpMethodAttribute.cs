using System;

namespace NovaCore.Web.Server.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class HttpMethodAttribute : Attribute
{
    public HttpMethodAttribute(HttpMethods httpMethod)
    {
        HttpMethod = httpMethod;
    }

    public HttpMethods HttpMethod { get; }
}