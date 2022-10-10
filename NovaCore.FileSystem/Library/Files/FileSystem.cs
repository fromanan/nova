using System;
using System.IO;
using NovaCore.Common.Utilities;
using Logger = NovaCore.Common.Logging.Logger;

namespace NovaCore.Files
{
    public static partial class FileSystem
    {
        public static readonly Logger Logger = new();

        public static bool IsPlatformValidOS()
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

        public static string TimestampFilename(string prefix = "", string separator = "_")
        {
            return $"{prefix}{separator}{Text.Timestamp()}";
        }

        public static string GuidFilename(string prefix = "", string separator = "_")
        {
            return $"{prefix}{separator}{Text.Guid()}";
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