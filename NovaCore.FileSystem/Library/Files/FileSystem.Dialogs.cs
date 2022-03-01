using System.Windows.Forms;

namespace NovaCore.Files
{
    public static partial class FileSystem
    {
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
        
        public static string SaveFileDialogue(FileFilter filter, string directory = null)
        {
            return RunFileDialog(new SaveFileDialog
            {
                DefaultExt = filter.DefaultExtension, 
                Filter = filter.ToString(), 
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
        
        public static string OpenFileDialogue(FileFilter filter, string directory = null)
        {
            return RunFileDialog(new OpenFileDialog
            {
                DefaultExt = filter.DefaultExtension,
                Filter = filter.Representation,
                InitialDirectory = string.IsNullOrEmpty(directory) ? Paths.Downloads : directory
            });
        }
        
        public static string[] OpenMultiFileDialogue(string defaultExtension = "", string filter = "", 
            string directory = null)
        {
            return RunMultiFileDialog(new OpenFileDialog
            {
                Multiselect = true,
                DefaultExt = defaultExtension,
                Filter = filter,
                InitialDirectory = string.IsNullOrEmpty(directory) ? Paths.Downloads : directory
            });
        }
        
        public static string[] OpenMultiFileDialogue(FileFilter filter, string directory = null)
        {
            return RunMultiFileDialog(new OpenFileDialog
            {
                Multiselect = true,
                DefaultExt = filter.DefaultExtension,
                Filter = filter.Representation,
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
                    RootFolder = SpecialFolder.MyComputer, 
                    ShowNewFolderButton = true
                };
                if (fbd.ShowDialog() == DialogResult.Cancel) return;
                selectedPath = fbd.SelectedPath;
            });

            return selectedPath;*/
            
            string selectedPath = null;
            
            RunSTA(() =>
            {
                FolderSelectDialog dialog = new()
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
                FolderSelectDialog dialog = new()
                {
                    Multiselect = true,
                    InitialDirectory = string.IsNullOrEmpty(directory) ? Paths.Downloads : directory
                };
                if (!dialog.ShowDialog()) return;
                selectedPaths = dialog.FileNames;
            });

            return selectedPaths;
        }
    }
}