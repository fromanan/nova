using System;
using System.Windows.Forms;

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
            return input == "Y" || input == "YE" || input == "YES";
        }
        
        public static void Notification(string title, string body)
        {
            switch (MessageBox.Show(body, title, MessageBoxButtons.OK))
            {
                case DialogResult.Yes:
                    //
                    break;
                case DialogResult.No:
                    //
                    break;
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    break;
                case DialogResult.Abort:
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Ignore:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void Confirmation(string title, string body)
        {
            switch (MessageBox.Show(body, title, MessageBoxButtons.YesNo))
            {
                case DialogResult.Yes:
                    //
                    break;
                case DialogResult.No:
                    //
                    break;
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    break;
                case DialogResult.Abort:
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Ignore:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void Alert()
        {
            
        }
    }
}