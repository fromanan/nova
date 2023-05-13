using System;
using System.IO;
using System.Windows.Forms;
using NovaCore.Common.Extensions;
using NovaCore.Common.Utilities;
using NovaCore.Files;


namespace NovaCore.Forms;

public static class NovaDialogs
{
    public static string? CreateFile(string extension = "", string filepath = "", string? directory = null)
    {
        // Filepath was provided
        if (string.IsNullOrEmpty(filepath))
        {
            filepath = SaveFileDialogue(new FileFilter(extension), directory);
            
            if (filepath.IsNullOrEmpty())
                return null;
            
            File.Create(filepath).Close();
            
            return filepath;
        }
            
        // File exists
        // TODO: Return the existing filename?
        if (FileSystem.Validate(filepath))
            return null;

        File.Create(filepath).Close();
            
        return FileSystem.Validate(filepath) ? filepath : null;
    }

    public static string BasicFileFilter(string extension) => new FileFilter(extension).ToString();

    // TODO: Does not support multiple extensions
    // TODO: Detect file prompt closing, catch the null operator - Verify for this purpose
    public static string LoadFile(string extension, string? startDirectory = null)
    {
        return OpenFileDialogue(new FileFilter(extension), startDirectory);
    }
        
    public static string[] LoadFiles(string extension, string? startDirectory = null)
    {
        // Return an empty array if file cancelled (for iteration)
        return OpenMultiFileDialogue(new FileFilter(extension), startDirectory) ?? Array.Empty<string>();
    }
        
    // TODO: Default Filenames

    public static string[] LoadJson(string? startDirectory = null)
    {
        return LoadFiles("json", startDirectory);
    }

    public static string[] LoadTxt(string? startDirectory = null)
    {
        return LoadFiles("txt", startDirectory);
    }
        
    public static string RunFileDialog<T>(T dialog) where T : FileDialog
    {
        string selectedPath = "";
            
        FileSystem.RunSTA(() => 
        {
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;
            selectedPath = dialog.FileName;
        });

        return selectedPath;
    }
        
    public static string[]? RunMultiFileDialog<T>(T dialog) where T : FileDialog
    {
        string[]? selectedPaths = null;
            
        FileSystem.RunSTA(() => 
        {
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;
            selectedPaths = dialog.FileNames;
        });

        return selectedPaths;
    }
        
    public static string SaveFileDialogue(string defaultExtension = "", string filter = "", string? directory = null)
    {
        return RunFileDialog(new SaveFileDialog
        {
            DefaultExt = defaultExtension, 
            Filter = filter, 
            InitialDirectory = directory.ValueOrDefault(Paths.Downloads),
            RestoreDirectory = true
        });
    }
        
    public static string SaveFileDialogue(FileFilter filter, string? directory = null)
    {
        return RunFileDialog(new SaveFileDialog
        {
            DefaultExt = filter.DefaultExtension, 
            Filter = filter.ToString(), 
            InitialDirectory = directory.ValueOrDefault(Paths.Downloads),
            RestoreDirectory = true
        });
    }
        
    public static string OpenFileDialogue(string defaultExtension = "", string filter = "", string? directory = null)
    {
        return RunFileDialog(new OpenFileDialog
        {
            DefaultExt = defaultExtension,
            Filter = filter,
            InitialDirectory = directory.ValueOrDefault(Paths.Downloads)
        });
    }
        
    public static string OpenFileDialogue(FileFilter filter, string? directory = null)
    {
        return RunFileDialog(new OpenFileDialog
        {
            DefaultExt = filter.DefaultExtension,
            Filter = filter.Representation,
            InitialDirectory = directory.ValueOrDefault(Paths.Downloads)
        });
    }
        
    public static string[]? OpenMultiFileDialogue(string defaultExtension = "", string filter = "", 
        string? directory = null)
    {
        return RunMultiFileDialog(new OpenFileDialog
        {
            Multiselect = true,
            DefaultExt = defaultExtension,
            Filter = filter,
            InitialDirectory = directory.ValueOrDefault(Paths.Downloads)
        });
    }
        
    public static string[]? OpenMultiFileDialogue(FileFilter filter, string? directory = null)
    {
        return RunMultiFileDialog(new OpenFileDialog
        {
            Multiselect = true,
            DefaultExt = filter.DefaultExtension,
            Filter = filter.Representation,
            InitialDirectory = directory.ValueOrDefault(Paths.Downloads)
        });
    }
        
    public static string? OpenFolderDialogue(string? directory = null)
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
            
        string? selectedPath = null;
            
        FileSystem.RunSTA(() =>
        {
            FolderSelectDialog dialog = new()
            {
                InitialDirectory = directory.ValueOrDefault(Paths.Downloads)
            };
                
            if (!dialog.ShowDialog())
                return;
                
            selectedPath = dialog.FileName;
        });

        return selectedPath;
    }
        
    public static string[]? OpenMultiFolderDialogue(string? directory = null)
    {
        string[]? selectedPaths = null;
            
        FileSystem.RunSTA(() =>
        {
            FolderSelectDialog dialog = new()
            {
                Multiselect = true,
                InitialDirectory = directory.ValueOrDefault(Paths.Downloads)
            };
                
            if (!dialog.ShowDialog())
                return;
                
            selectedPaths = dialog.FileNames;
        });

        return selectedPaths;
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
            case DialogResult.TryAgain:
            case DialogResult.Continue:
            default:
                break;
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

    public static void Alert()
    {
        throw new NotImplementedException();
    }
}