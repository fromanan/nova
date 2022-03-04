namespace NovaCore.Logging
{
    public partial class Debug
    {
        public static string Prompt(string message, bool useCount = true)
        {
            StandardOutput.WriteLine(useCount ? $"< {count++} > [{message}]" : $"{message}:");
            string input = Input();
            LineBreak();
            return input;
        }

        public static string Input(string message = "> ")
        {
            StandardOutput.Write(message);
            SetTextColor(InputColor);
            string input = StandardInput.ReadLine();
            SetTextColor();
            return input;
        }

        public static bool Confirm(string action)
        {
            StandardOutput.WriteLine($"Are you sure you want to {action}? [Y/N]");
            string input = Input().ToUpper();
            return input is "Y" or "YE" or "YES";
        }
    }
}