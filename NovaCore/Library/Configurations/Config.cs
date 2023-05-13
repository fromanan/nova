using System;
using System.IO;
using Newtonsoft.Json;
using NovaCore.Common;
using NovaCore.Common.Debugging;
using NovaCore.Common.Utilities;
using NovaCore.Core;
using NovaCore.Files;

namespace NovaCore.Configurations;

[Serializable]
public class Config : ConfigFile
{
    private readonly string Filepath = FileSystem.CreateFilepath($"{Paths.Root}.config", 
        Paths.Settings);

    public override string GetFilepath() => Filepath;
        
    [JsonProperty("debug_mode")] 
    public Debug.Mode DebugMode = Debug.Mode.DEFAULT;

    [JsonProperty("default_save_path")]
    public string DefaultSavePath = Paths.ProjectDownloads;

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
            Paths.ProjectDownloads :
            savePath;
    }
}