using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigJsonNET.ConfigurationHelper
{
    public static class AppUtility
    {
        internal static T CopyObject<T>(T data)
        {
            return FileHandler.JsonConvertDeSerialize<T>(FileHandler.JsonConvertSerializeObject(data));
        }

        private static string AppConfigLocation { set; get; }


      

        internal static string Persist<T>(T obj, string file,string key)
        {
            var json = FileHandler.JsonConvertSerializeObject(obj);
            FileHandler.WriteAllText(file, json, key);
            return FileHandler.ReadAllText(file, key);
        }

        private static T InitializeConfig<T>(string file, T init,string key) where T : new()
        {
            CheckAndGenerateConfigFilesIfNotInExistence(file, init, key);
            var content = FileHandler.ReadAllText(file, key);
            var hasContent = !string.IsNullOrEmpty(content);
            var existsAndHasContent = (FileHandler.Exists(file, key) && hasContent);
            if (!existsAndHasContent)
            {
                content = Persist(init, file, key);
            }

            if (string.IsNullOrEmpty(content)) return default(T);

            var setUpFileList = FileHandler.JsonConvertDeSerialize<T>(content);

            return setUpFileList;
        }

        private static void CheckAndGenerateConfigFilesIfNotInExistence<T>(string file, T init,string key) where T : new()
        {
            if (string.IsNullOrEmpty(file)) throw new Exception("file");

            if (!FileHandler.Exists(file, key))
            {

                Persist(init, file, key);
            }
        }

        public static IConfigurationModuleFileHandler FileHandler { set; get; }

        public static T LoadAppConfiguration<T>() where T : new()
        {
            var firstActiveConfig = GetFirstActiveConfig<T>();
            AppConfigLocation = firstActiveConfig.FileName;

            if (string.IsNullOrEmpty(firstActiveConfig.BaseDir)) throw new Exception("MissingBase Dir");
            if (string.IsNullOrEmpty(AppConfigLocation)) throw new Exception("Name missing from active configuration");

            AppConfigLocation = firstActiveConfig.FileName;

            // return Convert.ChangeType((dynamic)obj, typeof(T));

            return InitializeConfig(firstActiveConfig.BaseDir + AppConfigLocation, new T(), typeof(T).FullName);
        }

        internal static SetUpFile GetFirstActiveConfig<T>() where T : new()
        {
            var t = FileHandler.SetUpFiles[typeof (T).FullName];
            if (t == null)
            {
                FileHandler.SetUpFiles[typeof(T).FullName] = ConfigJson<T>.SetUpFile;
            }
            var setupFileConfigs = InitializeConfig(ConfigJson<T>.PathToSetupFile, ConfigJson<T>.InitialSetUpFileObject, typeof(T).FullName);
            if(setupFileConfigs==null)
                setupFileConfigs=new List<SetUpFile>();
            var firstActiveConfig = string.IsNullOrEmpty(FileHandler.Selector[typeof(T).FullName]) ? setupFileConfigs.FindAll(x => x.IsActive).FirstOrDefault() : setupFileConfigs.FindAll(x => x.Selector == FileHandler.Selector[typeof(T).FullName]).FirstOrDefault();

            if (firstActiveConfig == null) return new SetUpFile();// throw new Exception("No Active Configuration Setup");
            return firstActiveConfig;
        }
    }
}