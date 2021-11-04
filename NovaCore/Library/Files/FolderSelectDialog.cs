using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace NovaCore.Library.Files
{
    // https://stackoverflow.com/questions/12261598/browse-multiple-folders-using-folderbrowserdialog-in-windows-application
    public class FolderSelectDialog
    {
        private readonly OpenFileDialog dialog;
        
        public FolderSelectDialog()
        {
            dialog = new OpenFileDialog
            {
                Filter = "Folders|\n",
                AddExtension = false,
                CheckFileExists = false,
                DereferenceLinks = true
            };
        }
        
        public string InitialDirectory
        {
            get => dialog.InitialDirectory;
            set => dialog.InitialDirectory = string.IsNullOrEmpty(value) ? Environment.CurrentDirectory : value;
        }
        public string Title
        {
            get => dialog.Title;
            set => dialog.Title = value ?? "Select a folder";
        }

        public bool Multiselect
        {
            get => dialog.Multiselect;
            set => dialog.Multiselect = value;
        }

        public string FileName => dialog.FileName;

        public string[] FileNames => dialog.FileNames;

        public bool ShowDialog()
        {
            return ShowDialog(IntPtr.Zero);
        }
        
        public bool ShowDialog(IntPtr hWndOwner)
        {
            bool flag;

            if (Environment.OSVersion.Version.Major >= 6)
            {
                Reflector reflector = new Reflector("System.Windows.Forms");

                Type typeIFileDialog = reflector.GetType("FileDialogNative.IFileDialog");
                
                object reflectorDialog = reflector.Call(dialog, "CreateVistaDialog");
                
                reflector.Call(dialog, "OnBeforeVistaDialog", reflectorDialog);
                
                uint options = (uint) reflector.CallAs(typeof(FileDialog), dialog, "GetOptions") | 
                               (uint) reflector.GetEnum("FileDialogNative.FOS", "FOS_PICKFOLDERS");
                
                reflector.CallAs(typeIFileDialog, reflectorDialog, "SetOptions", options);
                
                object dialogEvents = reflector.New("FileDialog.VistaDialogEvents", dialog);
                
                object[] parameters = { dialogEvents, (uint) 0 };
                reflector.CallAs2(typeIFileDialog, reflectorDialog, "Advise", parameters);
                
                try
                {
                    flag = 0 == (int) reflector.CallAs(typeIFileDialog, reflectorDialog, "Show", hWndOwner);
                }
                finally
                {
                    reflector.CallAs(typeIFileDialog, reflectorDialog, "Unadvise", (uint) parameters[1]);
                    GC.KeepAlive(dialogEvents);
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog
                {
                    Description = Title,
                    SelectedPath = InitialDirectory,
                    ShowNewFolderButton = false
                };
                
                if (fbd.ShowDialog(new WindowWrapper(hWndOwner)) != DialogResult.OK)
                {
                    return false;
                }
                dialog.FileName = fbd.SelectedPath;
                flag = true;
            }
            
            return flag;
        }

        public class WindowWrapper : IWin32Window
        {
            public WindowWrapper(IntPtr handle)
            {
                Handle = handle;
            }
            
            public IntPtr Handle { get; }
        }

        public class Reflector
        {
            private readonly string @namespace;

            private readonly Assembly assembly;
            
            public Reflector(string @namespace) : this(@namespace, @namespace) { }
            
            public Reflector(string assembly, string @namespace)
            {
                this.@namespace = @namespace;
                this.assembly = null;
                foreach (AssemblyName assemblyName in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
                {
                    if (!assemblyName.FullName.StartsWith(assembly)) continue;
                    this.assembly = Assembly.Load(assemblyName);
                    break;
                }
            }
            
            public Type GetType(string typeName)
            {
                Type type = null;
                
                string[] names = typeName.Split('.');

                if (names.Length > 0)
                {
                    type = assembly.GetType($"{@namespace}.{names[0]}");
                }

                type = names.Skip(1).Aggregate(type, (current, name) => 
                    current?.GetNestedType(name, BindingFlags.NonPublic));
                
                return type;
            }
            
            public object New(string name, params object[] parameters)
            {
                Type type = GetType(name);
                ConstructorInfo[] constructorInfos = type.GetConstructors();
                foreach (ConstructorInfo constructorInfo in constructorInfos)
                {
                    try
                    {
                        return constructorInfo.Invoke(parameters);
                    }
                    catch
                    {
                        // ignored
                    }
                }

                return null;
            }
            
            public object Call(object obj, string func, params object[] parameters)
            {
                return Call2(obj, func, parameters);
            }
            
            public object Call2(object obj, string func, object[] parameters)
            {
                return CallAs2(obj.GetType(), obj, func, parameters);
            }
            
            public object CallAs(Type type, object obj, string func, params object[] parameters)
            {
                return CallAs2(type, obj, func, parameters);
            }

            private const BindingFlags Bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            
            public object CallAs2(Type type, object obj, string func, object[] parameters)
            {
                MethodInfo methInfo = type.GetMethod(func, Bindings);
                return methInfo.Invoke(obj, parameters);
            }
            
            public object Get(object obj, string prop)
            {
                return GetAs(obj.GetType(), obj, prop);
            }
            
            public object GetAs(Type type, object obj, string prop)
            {
                PropertyInfo propInfo = type.GetProperty(prop, Bindings);
                return propInfo.GetValue(obj, null);
            }
            
            public object GetEnum(string typeName, string name)
            {
                Type type = GetType(typeName);
                FieldInfo fieldInfo = type.GetField(name);
                return fieldInfo.GetValue(null);
            }
        }
    }
}