using System;
using System.IO;
using NovaCore.Library.Files;
using UnityEngine;
using Guid = NovaCore.Library.Utilities.Guid;
using Object = UnityEngine.Object;

namespace Nova.Library.Files
{
    public class Asset : Object
    {
        public System.Guid guid { get; }
    
        private string filepath;
    
        public string Filepath => filepath;
    
        private DateTime dateCreated;
        private DateTime dateModified;
    
        public DateTime DateCreated => dateCreated;
    
        public DateTime DateModified => dateModified;

        private const string EXTENSION = "nva";

        public virtual string Extension => EXTENSION;
    
        // Set the Filepath Attribute and Decode a File to the Class if One Exists
        public Asset(string assetPath)
        {
            guid = Guid.Generate();
            CreateEntry(assetPath);
        }
    
        public Asset(string assetPath, string guid)
        {
            this.guid = Guid.Load(guid);
            CreateEntry(assetPath);
        }
    
        private void CreateEntry(string assetPath)
        {
            filepath = FileSystem.BuildPath(assetPath, $"{guid}.{Extension}");
            
            dateCreated = File.GetCreationTime(filepath);
            dateModified = File.GetLastWriteTime(filepath);
            
            if (File.Exists(filepath)) Decode();
            else Encode();
        }
    
        // Encode to File When Object is Destroyed
        ~Asset()
        {
            Encode();
        }
        
        public void Encode()
        {
            using (FileStream fs = FileSystem.OpenAssetStream(filepath))
            {
                using (BinaryWriter writer = FileSystem.OpenBinaryWriter(fs))
                {
                    writer.Write($"{guid}");
                }
            }
        }
    
        public void Decode()
        {
            using (FileStream fs = FileSystem.OpenAssetStream(filepath))
            {
                using (BinaryReader reader = FileSystem.OpenBinaryReader(fs))
                {
                    Debug.Log(reader.ReadString());
                }
            }
        }
    }
}
