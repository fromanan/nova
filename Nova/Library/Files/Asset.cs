using System;
using System.IO;
using UnityEngine;
using static NovaCore.Library.Files.FileSystem;
using Guid = NovaCore.Library.Utilities.Guid;
using Object = UnityEngine.Object;

namespace Nova.Library.Files
{
    public class Asset : Object
    {
        public System.Guid guid { get; private set; }
        public string Filepath { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime DateModified { get; private set; }

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
            Filepath = BuildPath(assetPath, $"{guid}.{Extension}");
            
            DateCreated = File.GetCreationTime(Filepath);
            DateModified = File.GetLastWriteTime(Filepath);
            
            if (File.Exists(Filepath)) Decode();
            else Encode();
        }
    
        // Encode to File When Object is Destroyed
        ~Asset()
        {
            Encode();
        }
        
        public void Encode()
        {
            using (FileStream fs = FileSystem.OpenAssetStream(Filepath))
            {
                using (BinaryWriter writer = FileSystem.OpenBinaryWriter(fs))
                {
                    writer.Write($"{guid}");
                }
            }
        }
    
        public void Decode()
        {
            using (FileStream fs = FileSystem.OpenAssetStream(Filepath))
            {
                using (BinaryReader reader = FileSystem.OpenBinaryReader(fs))
                {
                    Debug.Log(reader.ReadString());
                }
            }
        }
    }
}
