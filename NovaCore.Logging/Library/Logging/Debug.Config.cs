using System;

namespace NovaCore.Logging
{
    public static partial class Debug
    {
        public const int LINE_LENGTH = 50;
        private const int MAX_HEADER = LINE_LENGTH - 8;
        
        public static bool CLIEnabled = true;
        public static bool Monochrome = false;
        
        public const ConsoleColor DEFAULT_TEXT_COLOR = ConsoleColor.White;
        public const ConsoleColor DEFAULT_BACKGROUND_COLOR = ConsoleColor.Black;

        public static ConsoleColor InputColor = ConsoleColor.Green;

        public static ChannelPalette ChannelPalette = new ChannelPalette();
    }
}