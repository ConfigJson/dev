using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigJsonNET.ConfigurationHelper
{
    public class ConfigurationModuleFileHandler<T> : IConfigurationModuleFileHandler
    {
        public ConfigurationModuleFileHandler()
        {
            InMemoryFileSystem = new Dictionary<string, string>();
        }

        /// <summary>
        /// Adds indentation and line breaks to output of JavaScriptSerializer
        /// </summary>
        public override string FormatJson(string jsonString)
        {
            var stringBuilder = new StringBuilder();

            var escaping = false;
            var inQuotes = false;
            var indentation = 0;

            foreach (var character in jsonString)
            {
                if (escaping)
                {
                    escaping = false;
                    stringBuilder.Append(character);
                }
                else
                {
                    switch (character)
                    {
                        case '\\':
                            escaping = true;
                            stringBuilder.Append(character);
                            break;

                        case '\"':
                            inQuotes = !inQuotes;
                            stringBuilder.Append(character);
                            break;

                        default:
                            if (!inQuotes)
                            {
                                if (character == ',')
                                {
                                    stringBuilder.Append(character);
                                    stringBuilder.Append("\r\n");
                                    stringBuilder.Append('\t', indentation);
                                }
                                else if (character == '[' || character == '{')
                                {
                                    stringBuilder.Append(character);
                                    stringBuilder.Append("\r\n");
                                    stringBuilder.Append('\t', ++indentation);
                                }
                                else if (character == ']' || character == '}')
                                {
                                    stringBuilder.Append("\r\n");
                                    stringBuilder.Append('\t', --indentation);
                                    stringBuilder.Append(character);
                                }
                                else if (character == ':')
                                {
                                    stringBuilder.Append(character);
                                    stringBuilder.Append('\t');
                                }
                                else
                                {
                                    stringBuilder.Append(character);
                                }
                            }
                            else
                            {
                                stringBuilder.Append(character);
                            }
                            break;
                    }
                }
            }

            return stringBuilder.ToString();
        }

        internal override sealed Dictionary<string, string> InMemoryFileSystem { get; set; }

        internal override Dictionary<string, bool> RunInMemory { get; set; }

        internal override Dictionary<string, bool> AllowOverwrite { get; set; }

        internal override Dictionary<string, string> Selector { get; set; }

        internal override Dictionary<string, string> SetUpFiles { get; set; }

        public override void WriteAllText(string fileName, string content, string key)
        {
            if (!AllowOverwrite[key])
            {
                return;
            }

            if (RunInMemory[key])
            {
                if (!Exists(fileName, key))
                {
                    if (AllowOverwrite[key])
                    {
                        InMemoryFileSystem.Add(fileName, content);
                    }
                }

                if (AllowOverwrite[key])
                {
                    InMemoryFileSystem[fileName] = content;
                }

                return;
            }

            File.WriteAllText(fileName, content);
        }

        public override string ReadAllText(string fileName, string key)
        {
            if (!RunInMemory[key])
            {
                if (!File.Exists(fileName))
                    File.Create(fileName).Dispose();
                return File.ReadAllText(fileName);
            }

            if (!InMemoryFileSystem.ContainsKey(fileName))
            {
                return "";
            }

            return InMemoryFileSystem[fileName];
        }

        public override bool Exists(string fileName, string key)
        {
            if (RunInMemory[key])
            {
                return InMemoryFileSystem.ContainsKey(fileName);
            }
            return File.Exists(fileName);
        }

        public override string JsonConvertSerializeObject<T>(T data)
        {
            return FormatJson(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(data));
            //  return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public override T JsonConvertDeSerialize<T>(string data)
        {
            return new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<T>(data);

            //  return JsonConvert.DeserializeObject<T>(data);
        }
    }
}