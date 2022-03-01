using System;
using System.IO;
using Newtonsoft.Json;

namespace NovaCore.Files
{
    public static partial class FileSystem
    {
        public static string SerializeToFile(object data, string filename, params string[] folderHierarchy)
        {
            return SaveToFile(Serialize(data), filename, folderHierarchy);
        }
        
        // Encode / Decode
        public static readonly JsonSerializerSettings DefaultSerializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            Error = (sender, args) =>
            {
                Logger.LogError(args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = true;
            }
        };
        
        public static T DeserializeFile<T>(string filename)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filename), DefaultSerializerSettings);
        }

        public static T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, DefaultSerializerSettings);
        }
        
        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, DefaultSerializerSettings);
        }
        
        public static bool TestDeserialize<T>(string filepath)
        {
            return Serialize(DeserializeFile<T>(filepath)) == File.ReadAllText(filepath);
        }

        public static bool TestSerialize<T>(string filepath)
        {
            T deserialized = DeserializeFile<T>(filepath);
            string path = Directory.GetParent(filepath)?.ToString() ?? "";
            try
            {
                SerializeToFile(deserialized, "test.json", path);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return false;
            }
            return true;
        }
    }
}