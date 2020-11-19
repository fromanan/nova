using Nova.Library.Files;
using UnityEditor;
using UnityEngine;

namespace NovaEditor.Files
{
    public static class EditorFileSystem
    {
        public static bool ValidateFolder(string folder, params string[] folderParentHierarchy)
        {
            string path = FileSystem.GeneratePath(folderParentHierarchy + folder);
            if (AssetDatabase.IsValidFolder(path)) return true;
            
            // TODO: Should include error checking (path could not be made)
            AssetDatabase.CreateFolder(path, folder);
            return true;
        }

        public static bool CreateAndSave(Object asset, string filename, string folder, params string[] folderParentHierarchy)
        {
            ValidateFolder(folder, folderParentHierarchy);
            AssetDatabase.CreateAsset(asset, $"Assets/Resources/{filename}.asset");
            AssetDatabase.SaveAssets();
            return true;
        }
    }
}