using System.Linq;
using Newtonsoft.Json;
using NovaCore.Files;
using Debug = NovaCore.Logging.Debug;

namespace NovaCore.Configurations
{
    public abstract class ConfigFile
    {
        public abstract string GetFilepath();

        [JsonIgnore] public string PathOnDisk => GetFilepath();
        
        [JsonIgnore] private string ClassName => GetType().FullName?
            .Split('.').Last()
            .Split('+').Last();
        
        public void Save()
        {
            FileSystem.SerializeToFile(this, PathOnDisk);
        }

        public T Load<T>() where T : new()
        {
            if (FileSystem.Verify(PathOnDisk)) return FileSystem.DeserializeFile<T>(PathOnDisk);
            Debug.LogWarning($"{ClassName} file not found, creating new one");
            return new T();
        }
    }
}