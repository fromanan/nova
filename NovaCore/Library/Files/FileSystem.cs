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
        
        /*public static async Task RunSTAAsync(ThreadStart threadStart)
        {
            Thread thread = new Thread(threadStart);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }*/
        
        public static string RunFileDialog<T>(T dialog) where T : FileDialog
        {
            string selectedPath = "";
            
            RunSTA(() => 
            {
                if (dialog.ShowDialog() == DialogResult.Cancel) return;
                selectedPath = dialog.FileName;
            });

            return selectedPath;
        }
        
        public static string[] RunMultiFileDialog<T>(T dialog) where T : FileDialog
        {
            string[] selectedPaths = null;
            
            RunSTA(() => 
            {
                if (dialog.ShowDialog() == DialogResult.Cancel) return;
                selectedPaths = dialog.FileNames;
            });

            return selectedPaths;
        }
        
        public static string SaveFileDialogue(string defaultExtension = "", string filter = "", string directory = null)
        {
            return RunFileDialog(new SaveFileDialog
            {
                DefaultExt = defaultExtension, 
                Filter = filter, 
                InitialDirectory = string.IsNullOrEmpty(directory) ? Paths.Downloads : directory,
                RestoreDirectory = true
            });
        }
        
        public static string OpenFileDialogue(string defaultExtension = "", string filter = "", string directory = null)
        {
            return RunFileDialog(new OpenFileDialog
            {
                DefaultExt = defaultExtension,
                Filter = filter,
                InitialDirectory = string.IsNullOrEmpty(directory) ? Paths.Downloads : directory
            });
        }
        
        public static string[] OpenMultiFileDialogue(string defaultExtension = "", string filter = "", string directory = null)
        {
            return RunMultiFileDialog(new OpenFileDialog
            {
                Multiselect = true,
                DefaultExt = defaultExtension,
                Filter = filter,
                InitialDirectory = string.IsNullOrEmpty(directory) ? Paths.Downloads : directory
            });
        }
        
        public static string OpenFolderDialogue(string directory = null)
        {
            /*string selectedPath = "";

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

            return selectedPath;*/
            
            string selectedPath = null;
            
            RunSTA(() =>
            {
                FolderSelectDialog dialog = new FolderSelectDialog
                {
                    InitialDirectory = string.IsNullOrEmpty(directory) ? Paths.Downloads : directory
                };
                if (!dialog.ShowDialog()) return;
                selectedPath = dialog.FileName;
            });

            return selectedPath;
        }
        
        public static string[] OpenMultiFolderDialogue(string directory = null)
        {
            string[] selectedPaths = null;
            
            RunSTA(() =>
            {
                FolderSelectDialog dialog = new FolderSelectDialog
                {
                    Multiselect = true,
                    InitialDirectory = string.IsNullOrEmpty(directory) ? Paths.Downloads : directory
                };
                if (!dialog.ShowDialog()) return;
                selectedPaths = dialog.FileNames;
            });

            return selectedPaths;
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
            // Filepath was provided
            if (string.IsNullOrEmpty(filepath))
            {
                filepath = SaveFileDialogue(extension, Filter(extension), directory);
                if (string.IsNullOrEmpty(filepath)) return null;
                File.Create(filepath).Close();
                return filepath;
            }
            
            // File exists
            if (Validate(filepath)) return null; //< TODO: Return the existing filename?
            
            File.Create(filepath).Close();
            
            return Validate(filepath) ? filepath : null;
        }

        public static string CreateTempFile()
        {
            return Path.GetTempFileName();
        }

        public static string Filter(string extension) => $"{extension} files (*.{extension})|*.{extension}";

        // TODO: Does not support multiple extensions
        // TODO: Detect file prompt closing, catch the null operator - Verify for this purpose
        public static string LoadFile(string extension, string startDirectory = null)
        {
            return OpenFileDialogue(extension, Filter(extension), startDirectory);
        }
        
        public static string[] LoadFiles(string extension, string startDirectory = null)
        {
            // Return an empty array if file cancelled (for iteration)
            return OpenMultiFileDialogue(extension, Filter(extension), startDirectory) ?? Array.Empty<string>();
        }
        
        // TODO: Default Filenames

        public static string[] LoadJson(string startDirectory = null)
        {
            return LoadFiles("json", startDirectory);
        }

        public static string[] LoadTxt(string startDirectory = null)
        {
            return LoadFiles("txt", startDirectory);
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

        public static string[] GetFiles(string directory, string searchPattern = "*")
        {
            return Directory.GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public static string[] GetAllFiles(string directory, string searchPattern = "*")
        {
            return Directory.GetFiles(directory, searchPattern, SearchOption.AllDirectories);
        }
        
        public static string[] GetSubdirectories(string directoryPath)
        {
            return Directory.GetDirectories(directoryPath);
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

        public static FileInfo GetFileInfo(string filepath)
        {
            return new FileInfo(filepath);
        }
        
        public static long GetFileSize(FileInfo fileInfo)
        {
            return fileInfo.Length;
        }
        
        public static long GetFileSize(string filepath)
        {
            return GetFileInfo(filepath).Length;
        }

        public static string FormatFileInfo(string filepath)
        {
            FileInfo fileInfo = GetFileInfo(filepath);
            return $"{fileInfo.Name} ({fileInfo.Length} bytes) : \"{filepath}\"";
        }
        
        // https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
        public static bool IsFileLocked(string filepath)
        {
            try
            {
                File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.None).Close();
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }
        
        public static bool IsFileLocked(FileInfo file)
        {
            try
            {
                file.Open(FileMode.Open, FileAccess.Read, FileShare.None).Close();
                return false;
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
        }
    }
}