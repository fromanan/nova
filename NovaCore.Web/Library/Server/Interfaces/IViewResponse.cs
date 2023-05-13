namespace NovaCore.Web.Server.Interfaces;

public interface IViewResponse
{
    string Body { get; }

    string ContentType { get; }
}