using System;
using System.IO;
using System.Linq;
using NovaCore.Common;

namespace NovaCore.Files
{
    public static partial class FileSystem
    {
        public static class Paths
        {
            // System Folders
            public static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            public static readonly string Common = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            public static readonly string Local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            public static readonly string Temp = Path.GetTempPath();
            public static readonly string Documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            public static readonly string Downloads = KnownFolders.GetPath(KnownFolder.Downloads);
        }

        public static char Separator => Path.PathSeparator;

        public static string Join(params string[] paths)
        {
            return Path.Combine(paths);
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
            return IsValidFilename(GetFilename(filepath)) && IsValidDirectory(GetDirectory(filepath));
        }
        
        public static bool ValidateDirectory(string directoryPath)
        {
            return IsValidDirectory(directoryPath);
        }

        public static string GetFilename(string filepath)
        {
            return Path.GetFileName(filepath);
        }

        public static string GetDirectory(string filepath)
        {
            return Path.GetDirectoryName(filepath);
        }
        
        public static string GetExtension(string filepath)
        {
            return Path.GetExtension(filepath).Substring(1);
        }

        public static string GetSimpleFilename(string filepath)
        {
            return Path.GetFileNameWithoutExtension(filepath);
        }

        public static string ChangeExtension(string filepath, string newExtension)
        {
            return $"{GetSimpleFilename(filepath)}.{newExtension}";
        }

        /// <summary>
        /// Returns if a generated file exists (used for verifying that a file was successfully downloaded, for example)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool Verify(string filename)
        {
            return Validate(filename) && Directory.Exists(GetDirectory(filename)) && File.Exists(filename);
        }
        
        public static bool VerifyDirectory(string directoryPath)
        {
            return ValidateDirectory(directoryPath) && Directory.Exists(directoryPath);
        }

        public static void Assert(string filename)
        {
            if (!Verify(filename))
            {
                throw new ApplicationException("Critical resource does not exist");
            }
        }
        
        public static void AssertDirectory(string directoryPath)
        {
            if (!VerifyDirectory(directoryPath))
            {
                throw new ApplicationException("Critical directory does not exist");
            }
        }

        public static string BuildPath(params string[] folderHierarchy)
        {
            return EmptyHierarchy(folderHierarchy) ? null : Join(folderHierarchy);
        }
        
        public static string BuildFilepath(string filename, params string[] folderHierarchy)
        {
            return EmptyHierarchy(folderHierarchy) ? filename : Join(folderHierarchy.Append(filename).ToArray());
        }

        public static bool EmptyHierarchy(string[] folderHierarchy)
        {
            return folderHierarchy is not { Length: > 0 };
        }

        public static string CreateFolder(params string[] folderHierarchy)
        {
            string directory = Join(folderHierarchy);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return directory;
        }
        
        public static string CreatePath(params string[] folderHierarchy)
        {
            return EmptyHierarchy(folderHierarchy) ? null : CreateFolder(folderHierarchy);
        }
        
        public static string CreateFilepath(string filename, params string[] folderHierarchy)
        {
            return EmptyHierarchy(folderHierarchy) ? filename : Join(CreateFolder(folderHierarchy), filename);
        }
    }
}