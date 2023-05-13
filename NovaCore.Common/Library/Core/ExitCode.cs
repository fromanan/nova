namespace NovaCore.Common;

public enum ExitCode : int 
{
    Success     = 0,
    Error       = 1,
    Critical    = 2,
    Crash       = 3,
    Normal      = 4,
    User        = 5,
    Restart     = 6,
    Killed      = 7
}