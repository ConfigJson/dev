using System.Collections.Generic;
using CuttingEdge.Conditions;
using Newtonsoft.Json;

namespace ConfigJson.NET.ConfigurationHelper
{
    public  class PersistedConfiguration<T> where T : new()
    {

        internal static  string _ConfigLocation = @"C:\Users\";

        public T InitialConfigurationLoaded { set; get; }

        public static IConfigurationModuleFileHandler FileHandler = new ConfigurationModuleFileHandler();
        public  _Advanced Advanced =new _Advanced();

        public   class _Advanced
       {
           public   Dictionary<string, string> GetInMemoryStorage()
           {
               return AppUtility.FileHandler.InMemoryFileSystem;
           }

            public string ConfigLocation
            {
                set
                {
                    _ConfigLocation = value;
                }
                get
                {

                    return _ConfigLocation;
                }  

            }

            public bool AllowOverwrite
            {
                set
                {
                    CheckAndUpdateFileHandler();
                    AppUtility.FileHandler.AllowOverwrite = value;
                }
                get
                {
                    CheckAndUpdateFileHandler();
                    return AppUtility.FileHandler.AllowOverwrite;
                } 
            }

            public   bool RunInMemory
           {
               set
               {
                   CheckAndUpdateFileHandler();
                   AppUtility.FileHandler.RunInMemory = value;
               }
               get
               {
                   CheckAndUpdateFileHandler();
                   return AppUtility.FileHandler.RunInMemory;
               }
           }
           public T PersistedData
           {
               set
               {

                   CheckAndUpdateFileHandler();
                   var activeConfig = AppUtility.GetFirstActiveConfig();
                   var activeFileName = activeConfig.FileName;
                   var baseDir = activeConfig.BaseDir;

                   AppUtility.Persist(AppUtility.CopyObject(value), baseDir + activeFileName);
               }
           }

          

       }
 private static void CheckAndUpdateFileHandler()
           {
               if (AppUtility.FileHandler == null)
               {
                   Condition.Requires(FileHandler).IsNotNull();
                   AppUtility.FileHandler = FileHandler;
               }
           }
       
        public T Data {
            
            get
            {

                CheckAndUpdateFileHandler();
                return AppUtility.LoadAppConfiguration<T>();
            }
        }
    }
}