using System;
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

        internal static string Persist<T>(T obj, string file)
        {
            var json = FileHandler.JsonConvertSerializeObject(obj);
            FileHandler.WriteAllText(file, json);
            return FileHandler.ReadAllText(file);
        }

        private static T InitializeConfig<T>(string file, T init) where T : new()
        {
            CheckAndGenerateConfigFilesIfNotInExistence(file, init);
            var content = FileHandler.ReadAllText(file);
            var hasContent = !string.IsNullOrEmpty(content);
            var existsAndHasContent = (FileHandler.Exists(file) && hasContent);
            if (!existsAndHasContent)
            {
                content = Persist(init, file);
            }

            if (string.IsNullOrEmpty(content)) throw new Exception("Error loading config file content");

            var setUpFileList = FileHandler.JsonConvertDeSerialize<T>(content);

            return setUpFileList;
        }

        private static void CheckAndGenerateConfigFilesIfNotInExistence<T>(string file, T init) where T : new()
        {
            if (string.IsNullOrEmpty(file)) throw new Exception("file");

            if (!FileHandler.Exists(file))
            {
                Persist(init, file);
            }
        }

        public static IConfigurationModuleFileHandler FileHandler { set; get; }

        public static T LoadAppConfiguration<T>() where T : new()
        {
            var firstActiveConfig = GetFirstActiveConfig();
            AppConfigLocation = firstActiveConfig.FileName;

            if (string.IsNullOrEmpty(firstActiveConfig.BaseDir)) throw new Exception("MissingBase Dir");
            if (string.IsNullOrEmpty(AppConfigLocation)) throw new Exception("Name missing from active configuration");

            AppConfigLocation = firstActiveConfig.FileName;

            // return Convert.ChangeType((dynamic)obj, typeof(T));

            return InitializeConfig(firstActiveConfig.BaseDir + AppConfigLocation, new T());
        }

        internal static SetUpFile GetFirstActiveConfig()
        {
            var setupFileConfigs = InitializeConfig(ConfigJson<dynamic>.PathToSetupFile, ConfigJson<dynamic>.InitialSetUpFileObject);

            var firstActiveConfig = string.IsNullOrEmpty(FileHandler.Selector) ? setupFileConfigs.FindAll(x => x.IsActive).FirstOrDefault() : setupFileConfigs.FindAll(x => x.Selector == FileHandler.Selector).FirstOrDefault();

            if (firstActiveConfig == null) throw new Exception("No Active Configuration Setup");
            return firstActiveConfig;
        }
    }
}