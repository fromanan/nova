namespace NovaCore.Web.Server.Interfaces;

public interface IController
{
    IPipeline Pipeline { get; }
}