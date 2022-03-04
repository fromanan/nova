using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NovaCore.Configurations;
using NovaCore.Files;

namespace NovaCore
{
    internal partial class Program
    {
        [Serializable]
        public sealed class Preferences : ConfigFile
        {
            public static Preferences Instance { get; private set; } = new();
            
            public override string GetFilepath() =>  Path.Combine(NovaApplication.Paths.Preferences, "preferences.json");
            
            [JsonProperty("index")] private Dictionary<string, PrefType> Index = new();
            [JsonProperty("strings")] private Dictionary<string, string> StringEntries = new();
            [JsonProperty("floats")] private Dictionary<string, float> FloatEntries = new();
            [JsonProperty("ints")] private Dictionary<string, int> IntEntries = new();
            [JsonProperty("entries")] private Dictionary<string, Entry> Entries = new();
            
            public enum PrefType
            {
                String, Float, Int, Entry
            }

            public static void SaveAll()
            {
                Instance.Save();
            }

            public static void LoadAll()
            {
                Instance = Instance.Load<Preferences>();
            }
            
            public static bool Exists(string key)
            {
                return Instance.Index.ContainsKey(key);
            }

            public static void DeleteAll()
            {
                Instance.Index.Clear();
                Instance.Index = new Dictionary<string, PrefType>();
                
                Instance.StringEntries.Clear();
                Instance.StringEntries = new Dictionary<string, string>();
                
                Instance.FloatEntries.Clear();
                Instance.FloatEntries = new Dictionary<string, float>();
                
                Instance.IntEntries.Clear();
                Instance.IntEntries = new Dictionary<string, int>();
                
                Instance.Entries.Clear();
                Instance.Entries = new Dictionary<string, Entry>();
            }

            public static void DeleteKey(string key)
            {
                if (!Exists(key))
                    return;

                switch (Instance.Index[key])
                {
                    case PrefType.Float:
                        Instance.FloatEntries.Remove(key);
                        break;
                    case PrefType.Int:
                        Instance.IntEntries.Remove(key);
                        break;
                    case PrefType.String:
                        Instance.StringEntries.Remove(key);
                        break;
                    case PrefType.Entry:
                        Instance.Entries.Remove(key);
                        break;
                }

                Instance.Index.Remove(key);
            }

            public static float? GetFloat(string key)
            {
                if (!Exists(key) || Instance.Index[key] != PrefType.Float)
                    return null;

                return Instance.FloatEntries[key];
            }

            public static int? GetInt(string key)
            {
                if (!Exists(key) || Instance.Index[key] != PrefType.Int)
                    return null;

                return Instance.IntEntries[key];
            }

            public static string GetString(string key)
            {
                if (!Exists(key) || Instance.Index[key] != PrefType.String)
                    return null;

                return Instance.StringEntries[key];
            }

            public static T GetEntry<T>(string key)
            {
                if (!Exists(key) || Instance.Index[key] != PrefType.Entry)
                    return default;

                return Entry.Decode<T>(Instance.Entries[key]);
            }

            public static void SetFloat(string key, float value)
            {
                DeleteKey(key);
                Instance.Index[key] = PrefType.Float;
                Instance.FloatEntries[key] = value;
            }

            public static void SetInt(string key, int value)
            {
                DeleteKey(key);
                Instance.Index[key] = PrefType.Int;
                Instance.IntEntries[key] = value;
            }

            public static void SetString(string key, string value)
            {
                DeleteKey(key);
                Instance.Index[key] = PrefType.String;
                Instance.StringEntries[key] = value;
            }

            public static void SetEntry(string key, object value)
            {
                DeleteKey(key);
                Instance.Index[key] = PrefType.Entry;
                Instance.Entries[key] = Entry.Encode(value);
            }
            
            public interface IJsonSerializable
            {
                string Serialize();
                object Deserialize();
            }
            
            // Allows for Custom Entries
            [Serializable]
            public sealed class Entry //: IJsonSerializable
            {
                [JsonProperty("properties")] public Dictionary<string, string> Properties = new();
                
                public static Entry Encode(object data)
                {
                    return FileSystem.Deserialize<Entry>(FileSystem.Serialize(data));
                }

                public static T Decode<T>(Entry data)
                {
                    return FileSystem.Deserialize<T>(FileSystem.Serialize(data));
                }

                /*public static string Data
                {
                    get => "";
                    set => value;
                }

                public string Serialize()
                {
                    
                }

                public object Deserialize()
                {
                    
                }*/
            }
        }
    }
}