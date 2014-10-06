using System.Collections.Generic;

namespace ConfigJsonNET.ConfigurationHelper
{
    public abstract class IConfigurationModuleFileHandler
    {
        internal abstract Dictionary<string, string> InMemoryFileSystem { set; get; }
        internal abstract bool RunInMemory { set; get; }

        internal abstract bool AllowOverwrite { set; get; }

        public abstract void WriteAllText(string fileName, string content);
        public abstract string ReadAllText(string fileName);
        public abstract bool Exists(string fileName);
        public abstract string JsonConvertSerializeObject<T>(T data);
        public abstract T JsonConvertDeSerialize<T>(string data);
    }
}