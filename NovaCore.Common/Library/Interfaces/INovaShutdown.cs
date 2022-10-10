namespace NovaCore.Common
{
    public interface INovaShutdown : INova
    {
        void OnShutdown();
    }
}