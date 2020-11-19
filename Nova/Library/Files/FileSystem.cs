namespace Nova.Library.Files
{
    using System.IO;
    using UnityEngine;
    using UnityEditor;

    public static class FileSystem
    {
        public static readonly string BASEPATH = Application.persistentDataPath;
        public static readonly char SEPARATOR = Path.DirectorySeparatorChar;
        
        public static string GeneratePath(params string[] folderHierarchy)
        {
            return string.Join(SEPARATOR.ToString(), folderHierarchy);
        }

        public static string GenerateAssetPath(string filename, string fileExtension, params string[] subfolders)
        {
            string folderHierarchy = string.Join(SEPARATOR.ToString(), subfolders);
            return $"{BASEPATH}{SEPARATOR}{folderHierarchy}{SEPARATOR}{filename}.{fileExtension}";
        }
        
        public static bool IsValidFilename(string filename)
        {
            return !string.IsNullOrEmpty(filename) && filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
        }
        
        #region FileStream Operations
        public static FileStream OpenAssetStream(string filepath)
        {
            return !File.Exists(filepath) ? File.Create(filepath) : File.Open(filepath, FileMode.Open);
        }

        public static BinaryWriter OpenBinaryWriter(FileStream fs)
        {
            return new BinaryWriter(fs);
        }

        public static BinaryReader OpenBinaryReader(FileStream fs)
        {
            return new BinaryReader(fs);
        }
        #endregion
    }
}