using System;

namespace NovaCore.Common.Logging;

public class ChannelPalette
{
    public ConsoleColor DefaultColor = ConsoleColor.White;
    public ConsoleColor InfoColor = ConsoleColor.Cyan;
    public ConsoleColor WarningColor = ConsoleColor.Yellow;
    public ConsoleColor ErrorColor = ConsoleColor.Red;
    public ConsoleColor ExceptionColor = ConsoleColor.DarkRed;
    public ConsoleColor CriticalColor = ConsoleColor.Magenta;
    public ConsoleColor CrashColor = ConsoleColor.DarkMagenta;
    public ConsoleColor CustomColor = ConsoleColor.White;
        
    public void SetTextColor(LogLevel logLevel, ConsoleColor? color = null)
    {
        switch (logLevel)
        {
            case LogLevel.Log:
                DefaultColor = color ?? ConsoleColor.White;
                break;
            case LogLevel.Info:
                InfoColor = color ?? ConsoleColor.Cyan;
                break;
            case LogLevel.Warning:
                WarningColor = color ?? ConsoleColor.Yellow;
                break;
            case LogLevel.Error:
                ErrorColor = color ?? ConsoleColor.Red;
                break;
            case LogLevel.Exception:
                ExceptionColor = color ?? ConsoleColor.DarkRed;
                break;
            case LogLevel.Critical:
                CriticalColor = color ?? ConsoleColor.Magenta;
                break;
            case LogLevel.Crash:
                CrashColor = color ?? ConsoleColor.DarkMagenta;
                break;
            case LogLevel.Custom:
                CustomColor = color ?? ConsoleColor.White;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
        }
    }
}