using System.IO;
using UnityEngine;

namespace Nova.Library.Files
{
    public static class NovaFileSystem
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
                return NovaCore.Library.Files.FileSystem.BuildPath(Persistent, Path.Combine(subfolders), $"{filename}.{fileExtension}");
            }
        }
    }
}