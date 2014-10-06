using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ConfigJsonNET.ConfigurationHelper
{
    public class ConfigurationModuleFileHandler : IConfigurationModuleFileHandler
    {
        public ConfigurationModuleFileHandler()
        {
            InMemoryFileSystem=new Dictionary<string, string>();
        }

        internal override Dictionary<string, string> InMemoryFileSystem { get; set; }
        internal override bool RunInMemory { get; set; }
        internal override bool AllowOverwrite { get; set; }

        public override void WriteAllText(string fileName, string content)
        {
            if (Exists(fileName) && !AllowOverwrite)
            {
                return;
            }
            if (RunInMemory)
            {
                if (InMemoryFileSystem.ContainsKey(fileName))
                {
                    InMemoryFileSystem[fileName] = content;
                }
                else
                {
                    InMemoryFileSystem.Add(fileName,content);
                }
                
                return;
            }

            File.WriteAllText(fileName,content);
        }

        public override string ReadAllText(string fileName)
        {
            if (RunInMemory)
            {
                if (!InMemoryFileSystem.ContainsKey(fileName))
                {
                    throw new FileNotFoundException(fileName);
                }


                return InMemoryFileSystem[fileName];
            }
            return   File.ReadAllText(fileName);
        }

        public override bool Exists(string fileName)
        {
            if (RunInMemory)
            {

                return InMemoryFileSystem.ContainsKey(fileName);
            }
            return  File.Exists(fileName);
        }

        public override string JsonConvertSerializeObject<T>(T data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public override T JsonConvertDeSerialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}