namespace NovaCore.Common.Interfaces;

public interface INovaShutdown : INova
{
    void OnShutdown();
}