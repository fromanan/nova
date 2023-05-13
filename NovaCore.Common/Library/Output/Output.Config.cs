using System;
using NovaCore.Common.Logging;

namespace NovaCore.Common;

public static partial class Output
{
    public const int LINE_LENGTH = 50;
    public const int MAX_HEADER = LINE_LENGTH - 8;
        
    public static bool UsingConsole = true;
    public static bool Monochrome = false;
        
    public const ConsoleColor DEFAULT_TEXT_COLOR = ConsoleColor.White;
    public const ConsoleColor DEFAULT_BACKGROUND_COLOR = ConsoleColor.Black;

    public static ChannelPalette ChannelPalette = new();
}