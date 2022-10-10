using System;

namespace NovaCore.Common.Logging
{
    public partial class Logger
    {
        public const int LINE_LENGTH = 50;
        public static int MAX_HEADER => LINE_LENGTH - 8;
        
        public void LineBreak()
        {
            OnLog.Invoke("\n");
        }
        
        public void Separator(bool lineBreak = false)
        {
            string separator = new('=', LINE_LENGTH);
            OnLog.Invoke(lineBreak ? $"{separator}\n" : separator);
        }

        public void Header(string headerName, ConsoleColor? color = null)
        {
            int headerLength = headerName.Length;
            
            if (headerLength > MAX_HEADER)
            {
                OnLogWarning.Invoke($"Header ignored, invalid length of {headerLength} (maximum of {MAX_HEADER} characters)");
                return;
            }
            
            // 50 (default line length) - 2 (outside space) - 2 (brackets) - 2 (inside space) => 44
            string side = new('=', (LINE_LENGTH - 6 - headerLength)/2);
            string text = headerLength % 2 == 0
                ? $"{side} [ {headerName} ] {side}"
                : $"{side} [ {headerName}  ] {side}";

            if (color is not null)
            {
                OnLogC.Invoke(text, (ConsoleColor) color);
            }
            else
            {
                OnLog.Invoke(text);
            }
        }
    }
}