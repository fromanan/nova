namespace NovaCore.Common
{
    public interface INovaExit : INova
    {
        void OnExit(ExitCode exitCode);
    }
}