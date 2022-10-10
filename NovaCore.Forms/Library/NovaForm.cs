using System.Windows.Forms;
using NovaCore.Common.Debugging;
using NovaCore.Files;

namespace NovaCore.Forms
{
    public static class NovaForm
    {
        public static void Restart()
        {
            Application.Restart();
        }
    
        public static void Focus()
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