using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Debug = NovaCore.Library.Logging.Debug;

namespace NovaCore.Library.Files
{
    public static class FileSystem
    {
        public static class Paths
        {
            // System Folders
            public static readonly string Project = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));
            public static readonly string Build = AppContext.BaseDirectory;
            public static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            public static readonly string Common = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            public static readonly string Local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            public static readonly string Temp = Path.GetTempPath();
            public static readonly string Documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            public static readonly string Downloads = KnownFolders.GetPath(KnownFolder.Downloads);
        }

        public static bool IsValidFilename(string filename)
        {
            return ValidString(filename, Path.GetInvalidFileNameChars());
        }

        public static bool IsValidDirectory(string directory)
        {
            return ValidString(directory, Path.GetInvalidPathChars());
        }

        public static bool ValidString(string str, char[] invalidChars)
        {
            return !string.IsNullOrEmpty(str) && str.IndexOfAny(invalidChars) < 0;
        }
        
        /// <summary>
        /// Returns if a given filepath has both a valid filename and a valid directory
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static bool Validate(string filepath)
        {
            return IsValidFilename(Path.GetFileName(filepath)) && IsValidDirectory(Path.GetDirectoryName(filepath));
        }

        /// <summary>
        /// Returns if a generated file exists (used for verifying that a file was successfully downloaded, for example)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool Verify(string filename)
        {
            return Validate(filename) && Directory.Exists(Path.GetDirectoryName(filename)) && File.Exists(filename);
        }

        public static string BuildPath(params string[] folderHierarchy)
        {
            if (folderHierarchy == null || folderHierarchy.Length <= 0)
            {
                return null;
            }
            
            string directory = Path.Combine(folderHierarchy);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
        
        public static string BuildFilepath(string filename, params string[] folderHierarchy)
        {
            if (folderHierarchy == null || folderHierarchy.Length <= 0)
                return filename;
            
            string directory = Path.Combine(folderHierarchy);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return Path.Combine(directory, filename);
        }

        // Builds Path from Hierarchy (implicitly makes directory if it does not exist), then Saves file to that location
        // Returns a token to verify that the file saved to the correct location
        public static string SaveToFile(string body, string filename, params string[] folderHierarchy)
        {
            string path = BuildFilepath(filename, folderHierarchy);
            File.WriteAllText(path, body);
            return path;
        }
        
        public static void RunExternalProcess(string path, string arguments, bool printResults = true)
        {
            using (Process process = new Process
            {
                StartInfo =
                {
                    FileName = path,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                    //WindowStyle = ProcessWindowStyle.Hidden,
                    //CreateNoWindow = false
                }
            })
            {
                process.Start();
                process.WaitForExit();
                if (printResults) Console.WriteLine(process.StandardOutput.ReadToEnd());
            }
        }
        
        // https://stackoverflow.com/questions/10788982/is-there-any-async-equivalent-of-process-start
        public static Task<int> RunExternalProcessAsync(string path, string arguments)
        {
            TaskCompletionSource<int> completionSource = new TaskCompletionSource<int>();
            
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = path,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                    //WindowStyle = ProcessWindowStyle.Hidden,
                    //CreateNoWindow = false
                },
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
            Thread thread = new Thread(threadStart);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
        
        public static string SaveFileDialogue(string defaultExtension = "", string filter = "", string directory = null)
        {
            string selectedPath = "";
            
            string initialDirectory = string.IsNullOrEmpty(directory) ? Paths.Downloads : directory;
            
            RunSTA(() => 
            {
                SaveFileDialog ofd = new SaveFileDialog
                {
                    DefaultExt = defaultExtension, 
                    Filter = filter, 
                    InitialDirectory = initialDirectory,
                    RestoreDirectory = true
                };
                if (ofd.ShowDialog() == DialogResult.Cancel) return;
                selectedPath = ofd.FileName;
            });

            return selectedPath;
        }
        
        /*public static string OpenFileDialogue(string defaultExtension = "", string filter = "")
        {
            string selectedPath = "";
            
            RunSTA(() => 
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    DefaultExt = defaultExtension, 
                    Filter = filter, 
                    InitialDirectory = Paths.Downloads
                };
                if (ofd.ShowDialog() == DialogResult.Cancel) return;
                selectedPath = ofd.FileName;
            });

            return selectedPath;
        }*/
        
        public static string[] OpenFileDialogue(string defaultExtension = "", string filter = "", string directory = null)
        {
            string[] selectedPath = null;
            
            string initialDirectory = string.IsNullOrEmpty(directory) ? Paths.Downloads : directory;
            
            RunSTA(() => 
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Multiselect = true, 
                    DefaultExt = defaultExtension, 
                    Filter = filter, 
                    InitialDirectory = initialDirectory
                };
                if (ofd.ShowDialog() == DialogResult.Cancel) return;
                selectedPath = ofd.FileNames;
            });

            return selectedPath;
        }
        
        // TODO: Support initial directory option
        public static string OpenFolderDialogue(string directory = null)
        {
            string selectedPath = "";

            RunSTA(() => 
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog
                {
                    RootFolder = Environment.SpecialFolder.MyComputer, 
                    ShowNewFolderButton = true
                };
                if (fbd.ShowDialog() == DialogResult.Cancel) return;
                selectedPath = fbd.SelectedPath;
            });

            return selectedPath;
        }

        #region Data Serialization

        public static string SerializeToFile(object data, string filename, params string[] folderHierarchy)
        {
            return SaveToFile(Serialize(data), filename, folderHierarchy);
        }
        
        // Encode / Decode
        public static readonly JsonSerializerSettings DefaultSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            Error = (sender, args) =>
            {
                Debug.LogError(args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = true;
            }
        };
        
        public static T DeserializeFile<T>(string filename)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filename), DefaultSerializerSettings);
        }

        public static T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, DefaultSerializerSettings);
        }
        
        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, DefaultSerializerSettings);
        }
        
        #endregion

        public static string ReadFile(string filepath)
        {
            return File.ReadAllText(filepath);
        }

        public static string CreateFile(string extension = "", string filepath = "", string directory = null)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                filepath = SaveFileDialogue(extension, Filter(extension), directory);
                if (!string.IsNullOrEmpty(filepath))
                {
                    using (File.Create(filepath))
                    {
                        return filepath;
                    }
                }
                return null;
            }
            
            // File exists
            if (Validate(filepath))
            {
                return null;
            }
            
            using (File.Create(filepath))
            {
                return Validate(filepath) ? filepath : null;
            }
        }

        public static string CreateTempFile()
        {
            return Path.GetTempFileName();
        }

        public static string Filter(string extension) => $"{extension} files (*.{extension})|*.{extension}";

        // TODO: Does not support multiple extensions
        // TODO: Detect file prompt closing, catch the null operator - Verify for this purpose
        public static string[] LoadFile(string extension, string startDirectory = null)
        {
            // Return an empty array to iterate if file cancelled
            return OpenFileDialogue(extension, Filter(extension), startDirectory) ?? Array.Empty<string>();
        }
        
        // TODO: Default Filenames

        public static string[] LoadJson(string startDirectory = null)
        {
            return LoadFile("json", startDirectory);
        }

        public static string[] LoadTxt(string startDirectory = null)
        {
            return LoadFile("txt", startDirectory);
        }
        
        public static bool LoadSuccessful(string[] filepaths)
        {
            return !(filepaths == null || filepaths.Length < 1 ||
                   filepaths.Length == 1 && string.IsNullOrEmpty(filepaths[0]));
        }

        // Copies a selected file to the downloads folder
        public static string Download(string filepath)
        {
            string filename = Path.Combine(Paths.Downloads, Path.GetFileName(filepath));
            File.Copy(filepath, filename);
            return filename;
        }

        // DateTime.Now:yyyyMMddHHmmssffff?
        public static string Timestamp()
        {
            return $"{DateTime.Now:yyyyMMddHHmmss}";
        }

        public static string TimestampFilename(string prefix = "", string separator = "_")
        {
            return $"{prefix}{separator}{Timestamp()}";
        }

        public static string Guid()
        {
            return $"{System.Guid.NewGuid()}";
        }

        public static string GuidFilename(string prefix = "", string separator = "_")
        {
            return $"{prefix}{separator}{Guid()}";
        }

        public static string[] GetFiles(string directory, string searchPattern)
        {
            return Directory.GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public static string[] GetAllFiles(string directory, string searchPattern)
        {
            return Directory.GetFiles(directory, searchPattern, SearchOption.AllDirectories);
        }

        public static void OpenExplorer(string arguments)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "explorer";
                process.StartInfo.Arguments = arguments;
                process.Start();
            }
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
    }
}