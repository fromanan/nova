using System;
using System.IO;
using Newtonsoft.Json;
using NovaCore.Common;
using NovaCore.Core;
using NovaCore.Files;
using Debug = NovaCore.Logging.Debug;

namespace NovaCore.Configurations
{
    [Serializable]
    public class Config : ConfigFile
    {
        private readonly string Filepath = FileSystem.CreateFilepath($"{NovaApp.Paths.Root}.config", 
            NovaApp.Paths.Settings);

        public override string GetFilepath() => Filepath;
        
        [JsonProperty("debug_mode")] 
        public Debug.Mode DebugMode = Debug.Mode.DEFAULT;

        [JsonProperty("media_space_mode")] 
        public MediaSpace.Mode MediaSpaceMode = MediaSpace.Mode.GENERAL;

        [JsonProperty("default_save_path")]
        public string DefaultSavePath = Path.Combine(FileSystem.Paths.Downloads, NovaApp.Paths.Root);

        [JsonProperty("update_number")] 
        public string UpdateNumber;

        [JsonProperty("version_guid")] 
        public string VersionGuid;

        [JsonProperty("last_updated")] 
        public string LastUpdated;

        public static void RegisterNewSystem()
        {
            // Send system guid to server, request SHA-256 signing with public key (private will be the guid)
        }

        public void SetDefaultSavePath(string savePath = null)
        {
            DefaultSavePath = string.IsNullOrEmpty(savePath) ? 
                Path.Combine(FileSystem.Paths.Downloads, NovaApp.Paths.Root) :
                savePath;
        }
    }
}