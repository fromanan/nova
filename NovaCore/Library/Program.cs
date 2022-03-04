using System;
using System.Windows.Forms;
using NovaCore.CLI;
using NovaCore.Files;
using Debug = NovaCore.Logging.Debug;

namespace NovaCore
{
    partial class Program
    {
        public static void Main(string[] args)
        {
            //StartupBeep();
            
            Init();

            using (Terminal terminal = new())
            {
                terminal.Run();
            }

            Save();
        }
        
        // TODO: Show Progress Bar
        public static void Init()
        {
            Debug.Clear();
            
            Debug.Log(NovaApplication.Copyright(), ConsoleColor.Cyan);

            Debug.Header("Initializing");

            Debug.Separator(true);

            // Shutdown Events
            AppDomain.CurrentDomain.ProcessExit += OnExit;
            Console.CancelKeyPress += OnCancel;
        }

        public static void Save()
        {
            
        }

        // Shutdown Procedure for Application
        public static void Close()
        {
            Save();
            //...
            Debug.LogInfo($"Informatix Closed ({DateTime.Now:G})");
        }

        // Corresponds to the ProcessExit event
        private static void OnExit(object sender, EventArgs e)
        {
            Close();
        }

        private static void OnCancel(object sender, EventArgs e)
        {
            // If none running, shutdown
            //Debug.LogInfo("Program Terminating - Reason: User Keyboard Interrupt\n");
            //ShutdownBeep();
            NovaApplication.Exit(ExitCode.User);
        }

        private static void Focus()
        {
            /*Form form = Form.ActiveForm;

            if (form is null)
            {
                Debug.LogError("Could not find active form");
                return;
            }
            
            // force window to have focus
            uint foreThread = Windowing.GetWindowThreadProcessId(Windowing.GetForegroundWindow(), IntPtr.Zero);
            uint appThread = Windowing.GetCurrentThreadId();
            const uint SW_SHOW = 5;
            if (foreThread != appThread)
            {
                Windowing.AttachThreadInput(foreThread, appThread, true);
                Windowing.BringWindowToTop(form.Handle);
                Windowing.ShowWindow(form.Handle, SW_SHOW);
                Windowing.AttachThreadInput(foreThread, appThread, false);
            }
            else
            {
                Windowing.BringWindowToTop(form.Handle);
                Windowing.ShowWindow(form.Handle, SW_SHOW);
            }
            form.Activate();*/

            Form.ActiveForm?.Focus();

            Windowing.SetForegroundWindow(Windowing.GetConsoleWindow());
            Debug.Log("Should be focused");
            
            /*string originalTitle = Console.Title;
            string uniqueTitle = Guid.NewGuid().ToString();
            Console.Title = uniqueTitle;
            Thread.Sleep(50);
            IntPtr handle = Windowing.FindWindowByCaption(IntPtr.Zero, uniqueTitle);

            if (handle == IntPtr.Zero)
            {
                Console.WriteLine("Oops, cant find main window.");
                return;
            }
            Console.Title = originalTitle;

            while (true)
            {
                Thread.Sleep(3000);
                Console.WriteLine(Windowing.SetForegroundWindow(handle));
            }*/
        }
    }
}