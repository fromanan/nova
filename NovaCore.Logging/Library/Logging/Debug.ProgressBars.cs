using System;
using System.Threading.Tasks;
using NovaCore.Utilities;

namespace NovaCore.Logging
{
    public partial class Debug
    {
        // https://stackoverflow.com/questions/24918768/progress-bar-in-console-application
        public static void ProgressBar(int progress, int total, ConsoleColor color = ConsoleColor.Green, 
            bool percent = true)
        {
            HideCursor();
            
            // Draw empty progress bar
            Console.CursorLeft = 0;
            StandardOutput.Write("["); // Start
            Console.CursorLeft = LINE_LENGTH - 1;
            StandardOutput.Write("]"); // End
            Console.CursorLeft = 1;
            float oneChunk = (float) (LINE_LENGTH - 2) / total;
            float fillMax = oneChunk * progress;

            // Draw filled part
            int position = 1;
            for (int i = 0; i < LINE_LENGTH - 2; i++)
            {
                //Console.BackgroundColor = i < fillMax ? ConsoleColor.Gray : ConsoleColor.Green;
                SetTextColor(i < fillMax ? color : ConsoleColor.DarkGray);
                Console.CursorLeft = position++;
                StandardOutput.Write("=");
            }

            // Draw totals
            Console.CursorLeft = LINE_LENGTH + 2;
            SetBackgroundColor();
            SetTextColor();
            int left = percent ? progress / total * 100 : progress;
            int right = percent ? 100 : total;
            StandardOutput.Write($"({left} / {right})\t"); // Blanks at the end remove any excess
            ShowCursor();
        }

        public static void FinishProgressBar(int total, ConsoleColor color = ConsoleColor.Green, bool percent = true)
        {
            ProgressBar(total, total, color, percent);
            LineBreak();
        }

        public static async Task TimedProgressBar(int duration = 1000, ConsoleColor color = ConsoleColor.Green)
        {
            for (int i = 0; i <= duration; i += 10)
            {
                ProgressBar(i, duration, color);
                await Tasks.Sleep(100);
            }
        }
        
        public static async Task ProgressBarTask(int duration, Task task, ConsoleColor color = ConsoleColor.Green)
        {
            if (task.IsCompleted || task.IsCanceled || task.IsFaulted) return;

            for (int i = 0; i <= duration; i += 10)
            {
                if (task.IsCompleted || task.IsCanceled || task.IsFaulted) break;
                ProgressBar(i, duration, color);
                await Tasks.Sleep(100);
            }

            while (!(task.IsCompleted || task.IsCanceled || task.IsFaulted))
            {
                await Tasks.Sleep(100);
            }
            
            ProgressBar(duration, duration, color);
            
            LineBreak();
        }

        public static async Task<T> ProgressBarTask<T>(int duration, Task<T> task, 
            ConsoleColor color = ConsoleColor.Green)
        {
            if (task.IsCompleted || task.IsCanceled || task.IsFaulted) return task.Result;

            for (int i = 0; i <= duration; i += 10)
            {
                if (task.IsCompleted || task.IsCanceled || task.IsFaulted) break;
                ProgressBar(i, duration, color);
                await Tasks.Sleep(100);
            }

            while (!(task.IsCompleted || task.IsCanceled || task.IsFaulted))
            {
                await Tasks.Sleep(100);
            }
            
            ProgressBar(duration, duration, color);
            
            LineBreak();

            return task.Result;
        }
    }
}