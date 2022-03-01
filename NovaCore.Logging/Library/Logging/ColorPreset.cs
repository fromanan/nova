using System;

namespace NovaCore.Logging
{
    public class ColorPreset : IDisposable
    {
        public ColorPreset(ConsoleColor color)
        {
            Debug.SetTextColor(color);
        }

        public void Dispose()
        {
            Debug.SetTextColor();
        }
    }
}