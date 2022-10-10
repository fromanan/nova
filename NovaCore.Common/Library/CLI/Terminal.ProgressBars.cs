using System;
using System.Threading.Tasks;
using NovaCore.Common.Utilities;

namespace NovaCore.Common.CLI
{
    public partial class Terminal
    {
        // https://stackoverflow.com/questions/24918768/progress-bar-in-console-application
        public static void ProgressBar(int progress, int total, ConsoleColor color = ConsoleColor.Green, 
            bool percent = true)
        {
            if (!Output.UsingConsole)
                throw new ApplicationException("Progress bars may only be used while in console mode");
            
            Input.HideCursor();
            
            // Draw empty progress bar
            Console.CursorLeft = 0;
            Output.Write("["); // Start
            Console.CursorLeft = Output.LINE_LENGTH - 1;
            Output.Write("]"); // End
            Console.CursorLeft = 1;
            float oneChunk = (float) (Output.LINE_LENGTH - 2) / total;
            float fillMax = oneChunk * progress;

            // Draw filled part
            int position = 1;
            for (int i = 0; i < Output.LINE_LENGTH - 2; i++)
            {
                //Console.BackgroundColor = i < fillMax ? ConsoleColor.Gray : ConsoleColor.Green;
                Output.SetTextColor(i < fillMax ? color : ConsoleColor.DarkGray);
                Console.CursorLeft = position++;
                Output.Write("=");
            }

            // Draw totals
            Console.CursorLeft = Output.LINE_LENGTH + 2;
            Output.SetBackgroundColor();
            Output.SetTextColor();
            int left = percent ? progress / total * 100 : progress;
            int right = percent ? 100 : total;
            Output.Write($"({left} / {right})\t"); // Blanks at the end remove any excess
            Input.ShowCursor();
        }

        public static void FinishProgressBar(int total, ConsoleColor color = ConsoleColor.Green, bool percent = true)
        {
            ProgressBar(total, total, color, percent);
            Output.LineBreak();
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
            
            Output.LineBreak();
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
            
            Output.LineBreak();

            return task.Result;
        }
    }
}