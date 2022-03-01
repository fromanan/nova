using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NovaCore.Common;

namespace NovaCore.Files
{
    public static partial class FileSystem
    {
        public static readonly Logger Logger = new();

        public static bool CheckValidOS()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return true;
                case PlatformID.Unix:
                case PlatformID.Xbox:
                case PlatformID.MacOSX:
                case PlatformID.Other:
                default:
                    return false;
            }
        }

        // DateTime.Now:yyyyMMddHHmmssffff?
        public static string Timestamp()
        {
            return $"{DateTime.Now:yyyyMMddHHmmss}";
        }

        public static string TimestampFilename(string prefix = "", string separator = "_")
        {
            return $"{prefix}{separator}{Timestamp()}";
        }

        public static string Guid()
        {
            return $"{System.Guid.NewGuid()}";
        }

        public static string GuidFilename(string prefix = "", string separator = "_")
        {
            return $"{prefix}{separator}{Guid()}";
        }

        public static FileInfo GetFileInfo(string filepath)
        {
            return new FileInfo(filepath);
        }
        
        public static long GetFileSize(FileInfo fileInfo)
        {
            return fileInfo.Length;
        }
        
        public static long GetFileSize(string filepath)
        {
            return GetFileInfo(filepath).Length;
        }

        public static string FormatFileInfo(string filepath)
        {
            FileInfo fileInfo = GetFileInfo(filepath);
            return $"{fileInfo.Name} ({fileInfo.Length} bytes) : \"{filepath}\"";
        }
    }
}