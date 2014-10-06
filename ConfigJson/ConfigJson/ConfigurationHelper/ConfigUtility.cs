using System.Linq;
using CuttingEdge.Conditions;

namespace ConfigJson.NET.ConfigurationHelper
{
    public static class AppUtility
    {
        internal static T CopyObject<T>(T data)
        {
            return AppUtility.FileHandler.JsonConvertDeSerialize<T>(AppUtility.FileHandler.JsonConvertSerializeObject(data));

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
            CheckAndGenerateConfigFilesIfNotInExistence(file,init);
            var content = FileHandler.ReadAllText(file);
            var hasContent = !string.IsNullOrEmpty(content);
            var existsAndHasContent = (FileHandler.Exists(file) && hasContent);
            if (!existsAndHasContent)
            {
                content = Persist(init, file);
            }

            Condition.Requires(content).IsNotNullOrEmpty("Error loading config file content");
            var setUpFileList = FileHandler.JsonConvertDeSerialize<T>(content);

            return setUpFileList;
        }

        private static void CheckAndGenerateConfigFilesIfNotInExistence<T>(string file, T init) where T : new()
        {
            Condition.Requires(string.IsNullOrEmpty(file));
            if (!FileHandler.Exists(file) )
            {
               
                Persist(init, file);
               
            }
        }

        public static IConfigurationModuleFileHandler FileHandler { set; get; }




        public static T LoadAppConfiguration<T>() where T : new()
        {
           
            var firstActiveConfig = GetFirstActiveConfig();
            AppConfigLocation = firstActiveConfig.FileName;
            Condition.Requires(firstActiveConfig.BaseDir).IsNotNullOrEmpty("MissingBase Dir");
            Condition.Requires(AppConfigLocation).IsNotNullOrEmpty("Name missing from active configuration");
            AppConfigLocation = firstActiveConfig.FileName;
            return InitializeConfig(firstActiveConfig.BaseDir + AppConfigLocation, new T());
        }

       

        internal static SetUpFile GetFirstActiveConfig() 
        {
            var setupFileConfigs = InitializeConfig(ConfigJson<dynamic>.PathToSetupFile, ConfigJson<dynamic>.InitialSetUpFileObject);
           
            var firstActiveConfig = setupFileConfigs.FindAll(x => x.IsActive).FirstOrDefault();

            Condition.Requires(firstActiveConfig).IsNotNull("No Active Configuration Setup");

           
            return firstActiveConfig;
        }
    }
}