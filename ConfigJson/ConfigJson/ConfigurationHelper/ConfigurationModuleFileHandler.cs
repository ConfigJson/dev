using System.Collections.Generic;
using System.IO;
using System.Text;


namespace ConfigJsonNET.ConfigurationHelper
{
    public class ConfigurationModuleFileHandler : IConfigurationModuleFileHandler
    {
        public ConfigurationModuleFileHandler()
        {
            InMemoryFileSystem=new Dictionary<string, string>();
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
        internal override bool RunInMemory { get; set; }
        internal override bool AllowOverwrite { get; set; }
        internal override string Selector { get; set; }
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
            if (!RunInMemory) return File.ReadAllText(fileName);
            if (!InMemoryFileSystem.ContainsKey(fileName))
            {
                throw new FileNotFoundException(fileName);
            }


            return InMemoryFileSystem[fileName];
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
            return FormatJson(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(data)) ;
          //  return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public override T JsonConvertDeSerialize<T>(string data)
        {
            return  new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<T>(data);
       
            //  return JsonConvert.DeserializeObject<T>(data);
        }
    }
}