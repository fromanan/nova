using System;

namespace NovaCore.Logging
{
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
        
        public void SetTextColor(Debug.LogType logType, ConsoleColor? color = null)
        {
            switch (logType)
            {
                case Debug.LogType.Log:
                    DefaultColor = color ?? ConsoleColor.White;
                    break;
                case Debug.LogType.Info:
                    InfoColor = color ?? ConsoleColor.Cyan;
                    break;
                case Debug.LogType.Warning:
                    WarningColor = color ?? ConsoleColor.Yellow;
                    break;
                case Debug.LogType.Error:
                    ErrorColor = color ?? ConsoleColor.Red;
                    break;
                case Debug.LogType.Exception:
                    ExceptionColor = color ?? ConsoleColor.DarkRed;
                    break;
                case Debug.LogType.Critical:
                    CriticalColor = color ?? ConsoleColor.Magenta;
                    break;
                case Debug.LogType.Crash:
                    CrashColor = color ?? ConsoleColor.DarkMagenta;
                    break;
                case Debug.LogType.Custom:
                    CustomColor = color ?? ConsoleColor.White;
                    break;
            }
        }
    }
}