namespace NovaCore.Common.Interfaces;

public interface INovaExit : INova
{
    void OnExit(ExitCode exitCode);
}