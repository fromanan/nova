using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NovaCore.Files
{
    public static partial class FileSystem
    {
        public static Process CreateExternalProcess(ProcessStartInfo startInfo)
        {
            return new Process { StartInfo = startInfo };
        }

        public static Process CreateExternalProcess(string path, string arguments = "")
        {
            return DefaultProcess(path, arguments);
        }
        
        public static Process DefaultProcess(string path, string args) => new()
        {
            StartInfo = DefaultStartInfo(path, args)
        };

        public static ProcessStartInfo DefaultStartInfo(string path, string args) => new()
        {
            FileName = path,
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
            //WindowStyle = ProcessWindowStyle.Hidden,
            //CreateNoWindow = false
        };
        
        public static void RunExternalProcess(string path, string arguments = "", bool printResults = true)
        {
            using Process process = DefaultProcess(path, arguments);
            process.Start();
            process.WaitForExit();
            if (printResults) Console.WriteLine(process.StandardOutput.ReadToEnd());
        }
        
        // https://stackoverflow.com/questions/10788982/is-there-any-async-equivalent-of-process-start
        public static Task<int> RunExternalProcessAsync(string path, string arguments = "")
        {
            TaskCompletionSource<int> completionSource = new();
            
            Process process = new()
            {
                StartInfo = DefaultStartInfo(path, arguments),
                EnableRaisingEvents = true
            };
            
            process.Exited += (sender, args) =>
            {
                completionSource.SetResult(process.ExitCode);
                process.Dispose();
            };

            process.Start();

            return completionSource.Task;
        }

        public static void RunSTA(ThreadStart threadStart)
        {
            Thread thread = new(threadStart);
            if (!CheckValidOS())
            {
                throw new PlatformNotSupportedException("RunSTA is not supported on Non-Windows systems");
            }
            #pragma warning disable CA1416
            thread.SetApartmentState(ApartmentState.STA);
            #pragma warning restore CA1416
            thread.Start();
            thread.Join();
        }
        
        /*public static async Task RunSTAAsync(ThreadStart threadStart)
        {
            Thread thread = new Thread(threadStart);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }*/
        
        public static void RunBasicProcess(string filepath, string arguments)
        {
            using Process process = DefaultProcess(filepath, arguments);
            process.Start();
        }

        public static void OpenExplorer(string arguments)
        {
            RunBasicProcess("explorer", arguments);
        }

        public static void OpenFolder(string path)
        {
            OpenExplorer($"\"{path}\"");
        }
        
        public static void OpenWithDefaultProgram(string filepath)
        {
            OpenExplorer($"\"{filepath}\"");
        }

        public static void ShowFileLocation(string filepath)
        {
            OpenExplorer($"/select,\"{filepath}\"");
        }

        public static Process OpenWithDefaultBrowser(string url)
        {
            return Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = url,
                Verb = "open"
            });
        }
        
        /*public static void OpenBrowser(string url)
        {
            
        }*/
    }
}