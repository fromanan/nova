using System;
using System.Linq;
using NovaCore.Common.Extensions;

namespace NovaCore.Files
{
    public class FileFilter : IFormattable
    {
        public readonly string Representation;
        
        public readonly string DefaultExtension;

        public FileFilter(string extension)
        {
            Representation = $"{extension} files (*.{extension})|*.{extension}";
            DefaultExtension = extension;
        }
        
        public FileFilter(string description, params string[] extensions)
        {
            string extensionsString = extensions.Select(s => $"*.{s}").Merge(";");
            Representation = $"{description}|{extensionsString}";
            DefaultExtension = extensions.First();
        }
        
        public FileFilter(string description, bool includeExtensionsInDescription, params string[] extensions)
        {
            string extensionsString = extensions.Select(s => $"*.{s}").Merge(";");
            string extraContent = includeExtensionsInDescription ? $" ({extensionsString})" : "";
            Representation = $"{description}{extraContent}|{extensionsString}";
            DefaultExtension = extensions.First();
        }

        public override string ToString()
        {
            return Representation;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Representation;
        }
    }
}