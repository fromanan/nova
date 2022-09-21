using System;
using System.Runtime.CompilerServices;

namespace NovaCore.Logging
{
    public partial class Debug
    {
        public static double ElapsedMilliseconds(DateTime startTime, DateTime? endTime = null)
        {
            return ((endTime ?? DateTime.Now) - startTime).TotalMilliseconds;
        }
        
        public static double ElapsedSeconds(DateTime startTime, DateTime? endTime = null)
        {
            return ((endTime ?? DateTime.Now) - startTime).TotalSeconds;
        }
        
        public static void Timestamp(DateTime startTime, DateTime? endTime = null)
        {
            StandardOutput.WriteLine($"Time Elapsed: {ElapsedSeconds(startTime, endTime):F2} seconds");
        }
        
        public static void LineBreak()
        {
            StandardOutput.WriteLine();
        }
        
        public static void Clear()
        {
            Console.Clear();
        }
        
        public static void SetTextColor(ConsoleColor color = DEFAULT_TEXT_COLOR)
        {
            Console.ForegroundColor = color;
        }

        public static void SetBackgroundColor(ConsoleColor color = DEFAULT_BACKGROUND_COLOR)
        {
            Console.BackgroundColor = color;
        }
        
        public static void Separator(bool lineBreak = false)
        {
            string separator = new('=', LINE_LENGTH);
            StandardOutput.WriteLine(lineBreak ? $"{separator}\n" : separator);
        }

        public static void Header(string headerName, ConsoleColor? color = null)
        {
            if (color != null) SetTextColor((ConsoleColor)color);
            
            int headerLength = headerName.Length;
            
            if (headerLength > MAX_HEADER)
            {
                LogWarning($"Header ignored, invalid length of {headerLength} (maximum of {MAX_HEADER} characters)");
                return;
            }
            
            // 50 (default line length) - 2 (outside space) - 2 (brackets) - 2 (inside space) => 44
            string side = new('=', (LINE_LENGTH - 6 - headerLength)/2);

            StandardOutput.WriteLine(headerLength % 2 == 0
                ? $"{side} [ {headerName} ] {side}"
                : $"{side} [ {headerName}  ] {side}");
            
            if (color != null) SetTextColor();
        }

        public static int LineNumber([CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }
    }
}