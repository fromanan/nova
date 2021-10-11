using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
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
            return !string.IsNullOrEmpty(filename) && filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
        }

        public static bool IsValidDirectory(string path)
        {
            return !string.IsNullOrEmpty(path) && path.IndexOfAny(Path.GetInvalidPathChars()) >= 0;
        }

        public static bool Verify(string filename)
        {
            if (File.Exists(filename)) return true;
            Debug.LogWarning($"File does not exist: \"{filename}\"");
            return false;
        }

        public static bool Validate(string filepath)
        {
            return filepath.IndexOfAny(Path.GetInvalidPathChars()) < 0 && 
                   Directory.Exists(Path.GetDirectoryName(filepath)) && 
                   File.Exists(filepath);
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

        public static void RunSTA(ThreadStart threadStart)
        {
            Thread thread = new Thread(threadStart);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
        
        public static string[] OpenFileDialogue(string defaultExtension = "", string filter = "")
        {
            string[] selectedPath = null;
            
            RunSTA(() => 
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Multiselect = true, DefaultExt = defaultExtension, Filter = filter, InitialDirectory = Paths.Downloads
                };
                if (ofd.ShowDialog() == DialogResult.Cancel) return;
                selectedPath = ofd.FileNames;
            });

            return selectedPath;
        }
        
        public static string OpenFolderDialogue()
        {
            string selectedPath = "";
            
            RunSTA(() => 
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog
                {
                    RootFolder = Environment.SpecialFolder.MyComputer, ShowNewFolderButton = true
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

        public static string CreateTempFile()
        {
            return Path.GetTempFileName();
        }

        // TODO: Does not support multi-extensions
        public static string[] LoadFile(string extension)
        {
            return OpenFileDialogue(extension, $"{extension} files (*.{extension})|*.{extension}") ?? 
                   Array.Empty<string>(); // Return an empty array to iterate if file cancelled
            
            // TODO: Detect file prompt closing, catch the null operator
        }
        
        // TODO: Default Filenames

        public static string[] LoadJson()
        {
            return LoadFile("json");
        }

        public static string[] LoadTxt()
        {
            return LoadFile("txt");
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