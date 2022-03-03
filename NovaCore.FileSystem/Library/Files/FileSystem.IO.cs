﻿using System;
using System.IO;
using System.Linq;

namespace NovaCore.Files
{
    public static partial class FileSystem
    {
        // Builds Path from Hierarchy (implicitly makes directory if it does not exist), then Saves file to that location
        // Returns a token to verify that the file saved to the correct location
        public static string SaveToFile(string body, string filename, params string[] folderHierarchy)
        {
            string path = BuildFilepath(filename, folderHierarchy);
            File.WriteAllText(path, body);
            return path;
        }
        
        public static string ReadFile(string filepath)
        {
            return File.ReadAllText(filepath);
        }

        public static string CreateFile(string extension = "", string filepath = "", string directory = null)
        {
            // Filepath was provided
            if (string.IsNullOrEmpty(filepath))
            {
                filepath = SaveFileDialogue(new FileFilter(extension), directory);
                if (string.IsNullOrEmpty(filepath))
                {
                    return null;
                }
                File.Create(filepath).Close();
                return filepath;
            }
            
            // File exists
            // TODO: Return the existing filename?
            if (Validate(filepath))
            {
                return null; 
            }
            
            File.Create(filepath).Close();
            
            return Validate(filepath) ? filepath : null;
        }

        public static string CreateTempFile()
        {
            return Path.GetTempFileName();
        }

        public static string BasicFileFilter(string extension) => new FileFilter(extension).ToString();

        // TODO: Does not support multiple extensions
        // TODO: Detect file prompt closing, catch the null operator - Verify for this purpose
        public static string LoadFile(string extension, string startDirectory = null)
        {
            return OpenFileDialogue(new FileFilter(extension), startDirectory);
        }
        
        public static string[] LoadFiles(string extension, string startDirectory = null)
        {
            // Return an empty array if file cancelled (for iteration)
            return OpenMultiFileDialogue(new FileFilter(extension), startDirectory) ?? Array.Empty<string>();
        }
        
        // TODO: Default Filenames

        public static string[] LoadJson(string startDirectory = null)
        {
            return LoadFiles("json", startDirectory);
        }

        public static string[] LoadTxt(string startDirectory = null)
        {
            return LoadFiles("txt", startDirectory);
        }
        
        public static bool LoadSuccessful(string[] filepaths)
        {
            return !(EmptyHierarchy(filepaths) || filepaths.Length == 1 && string.IsNullOrEmpty(filepaths[0]));
        }

        // Copies a selected file to the downloads folder
        public static string Download(string filepath)
        {
            string filename = Join(Paths.Downloads, Path.GetFileName(filepath));
            File.Copy(filepath, filename);
            return filename;
        }

        public static string[] GetFiles(string directory, string searchPattern = "*")
        {
            return Directory.GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public static string[] GetAllFiles(string directory, string searchPattern = "*")
        {
            return Directory.GetFiles(directory, searchPattern, SearchOption.AllDirectories);
        }
        
        public static string[] GetSubdirectories(string directoryPath)
        {
            return Directory.GetDirectories(directoryPath);
        }
        
        // Source: https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
        // Simplified syntax
        public static bool IsFileLocked(string filepath)
        {
            try
            {
                File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.None).Close();
                return false;
            }
            catch (IOException)
            {
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return true;
            }
        }
        
        // TODO: Return int instead indicating the failure type?
        public static bool IsFileLocked(FileInfo file)
        {
            try
            {
                file.Open(FileMode.Open, FileAccess.Read, FileShare.None).Close();
                return false;
            }
            catch (IOException)
            {
                /* The file is unavailable because it is:
                 *  - still being written to
                 *  - or being processed by another thread
                 *  - or does not exist (has already been processed) */
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return true;
            }
        }
        
        public static string GetNewestFile(string directory, string extension)
        {
            return !Directory.Exists(directory) ? null : new DirectoryInfo(directory)
                .EnumerateFiles()
                .Where(f => f.Extension.ToLower() == extension)
                .OrderByDescending(f => f.CreationTime)
                .FirstOrDefault()?.FullName;
        }
        
        public static string Save(string body, string name, string extension, params string[] folderHierarchy)
        {
            return SaveToFile(body, $"{TimestampFilename(name)}.{extension}", folderHierarchy);
        }
        
        public static string Download(string body, string name, string extension, params string[] folderHierarchy)
        {
            return SaveToFile(body, $"{TimestampFilename(name)}.{extension}",
                folderHierarchy.Prepend(Paths.Downloads).ToArray());
        }
    }
}