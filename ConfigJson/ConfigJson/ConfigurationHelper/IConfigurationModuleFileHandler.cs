using System.Collections.Generic;

namespace ConfigJsonNET.ConfigurationHelper
{
    public abstract class IConfigurationModuleFileHandler
    {
        internal abstract Dictionary<string, string> InMemoryFileSystem { set; get; }
        public abstract bool Exists(string fileName,string key);

        internal abstract Dictionary<string, string> SetUpFiles { set; get; }
        internal abstract Dictionary<string ,bool> RunInMemory { set; get; }

        internal abstract Dictionary<string, bool> AllowOverwrite { set; get; }

        internal abstract Dictionary<string, string> Selector { get; set; }

        public abstract void WriteAllText(string fileName, string content, string key);

        public abstract string ReadAllText(string fileName, string key);

    

        public abstract string JsonConvertSerializeObject<T>(T data);

        public abstract T JsonConvertDeSerialize<T>(string data);

        public abstract string FormatJson(string jsonString);
    }
}