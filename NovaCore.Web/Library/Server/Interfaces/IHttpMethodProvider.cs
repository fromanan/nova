namespace NovaCore.Web.Server.Interfaces;

public interface IHttpMethodProvider
{
    HttpMethods Provide(string name);
}