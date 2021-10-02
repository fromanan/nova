using System.IO;
using UnityEngine;
using static NovaCore.Library.Files.FileSystem;

namespace Nova.Library.Files
{
    public static class FileSystem
    {
        public static class Paths
        {
            public static readonly string Persistent = Application.persistentDataPath;
            public static readonly string StreamingAssets = Application.streamingAssetsPath;
            public static readonly string Data = Application.dataPath;
            public static readonly string Log = Application.consoleLogPath;
            public static readonly string Temp = Application.temporaryCachePath;

            public static string GenerateAssetPath(string filename, string fileExtension, params string[] subfolders)
            {
                return BuildPath(Persistent, Path.Combine(subfolders), $"{filename}.{fileExtension}");
            }
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