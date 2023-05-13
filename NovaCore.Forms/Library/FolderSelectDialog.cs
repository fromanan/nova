using System;
using System.Windows.Forms;
using NovaCore.Common.Extensions;
using NovaCore.Common.Utilities;

namespace NovaCore.Forms;

// https://stackoverflow.com/questions/12261598/browse-multiple-folders-using-folderbrowserdialog-in-windows-application
public class FolderSelectDialog
{
    #region Data Members

    private readonly OpenFileDialog _dialog;
    
    private readonly Reflector _formsReflector = new("System.Windows.Forms");

    #endregion
    
    #region Constructor

    public FolderSelectDialog()
    {
        _dialog = new OpenFileDialog
        {
            Filter = @"Folders|\n",
            AddExtension = false,
            CheckFileExists = false,
            DereferenceLinks = true
        };
    }

    #endregion

    #region Attributes

    public string InitialDirectory
    {
        get => _dialog.InitialDirectory;
        set => _dialog.InitialDirectory = value.ValueOrDefault(Paths.Downloads);
    }
    public string Title
    {
        get => _dialog.Title;
        set => _dialog.Title = value.ValueOrDefault("Select a folder");
    }

    public bool Multiselect
    {
        get => _dialog.Multiselect;
        set => _dialog.Multiselect = value;
    }

    public string FileName => _dialog.FileName;

    public string[] FileNames => _dialog.FileNames;

    #endregion
    
    public bool ShowDialog()
    {
        return ShowDialog(IntPtr.Zero);
    }

    private bool ShowDialog(IntPtr hWndOwner)
    {
        bool flag;
        
        if (Environment.OSVersion.Version.Major < 6)
            return OpenFolderBrowserDialog(hWndOwner);

        if (_formsReflector.GetType("FileDialogNative.IFileDialog") is not { } typeIFileDialog)
            return false;

        if (Reflector.Call(_dialog, "CreateVistaDialog") is not { } reflectorDialog)
            return false;

        Reflector.Call(_dialog, "OnBeforeVistaDialog", reflectorDialog);

        if (Reflector.CallAs(typeof(FileDialog), _dialog, "GetOptions") is not { } getOptions)
            return false;

        if (_formsReflector.GetEnum("FileDialogNative.FOS", "FOS_PICKFOLDERS") is not { } pickFolders)
            return false;
                
        uint options = (uint) getOptions | (uint) pickFolders;
                
        Reflector.CallAs(typeIFileDialog, reflectorDialog, "SetOptions", options);

        if (_formsReflector.New("FileDialog.VistaDialogEvents", _dialog) is not { } dialogEvents)
            return false;
                
        object[] parameters =
        {
            dialogEvents,
            0u
        };
        
        Reflector.CallAs(typeIFileDialog, reflectorDialog, "Advise", parameters);
                
        try
        {
            if (Reflector.CallAs(typeIFileDialog, reflectorDialog, "Show", hWndOwner) is not { } showResult)
                return false;
            flag = (int)showResult == 0;
        }
        finally
        {
            Reflector.CallAs(typeIFileDialog, reflectorDialog, "Unadvise", (uint) parameters[1]);
            GC.KeepAlive(dialogEvents);
        }
            
        return flag;
    }

    private bool OpenFolderBrowserDialog(IntPtr hWndOwner)
    {
        FolderBrowserDialog fbd = new()
        {
            Description = Title,
            SelectedPath = InitialDirectory,
            ShowNewFolderButton = false
        };
                
        if (fbd.ShowDialog(new WindowWrapper(hWndOwner)) != DialogResult.OK)
            return false;
            
        _dialog.FileName = fbd.SelectedPath;
            
        return true;
    }
        
    public record WindowWrapper(IntPtr Handle) : IWin32Window;
}