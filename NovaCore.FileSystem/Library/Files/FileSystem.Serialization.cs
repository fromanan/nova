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
            return JsonConvert.DeserializeObject<T>(ReadFile(filename), DefaultSerializerSettings);
        }

        public static T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, DefaultSerializerSettings);
        }
        
        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, DefaultSerializerSettings);
        }
        
        /// <summary>
        /// Deserializes form a filepath first, then attempts to serialize it
        ///     and compares contents to the original file read
        /// </summary>
        /// <param name="filepath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TestDeserialize<T>(string filepath)
        {
            return Serialize(DeserializeFile<T>(filepath)) == ReadFile(filepath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TestSerialize<T>(string filepath)
        {
            string contents = ReadFile(filepath);
            T deserialized = Deserialize<T>(contents);
            try
            {
                SerializeToFile(deserialized, CreateTempFile());
                if (ReadFile(filepath) == contents) return true;
                Logger.LogError("Deserialization passed, but information was lost when re-serializing data");
                return false;
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                return false;
            }
        }
    }
}