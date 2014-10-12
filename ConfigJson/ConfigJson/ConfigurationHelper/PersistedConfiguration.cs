using System;
using System.Collections.Generic;

namespace ConfigJsonNET.ConfigurationHelper
{
    public class PersistedConfiguration<T> where T : new()
    {
        internal static string _ConfigLocation = @"C:\Users\";

        public T InitialConfigurationLoaded { set; get; }

        public static IConfigurationModuleFileHandler FileHandler = new ConfigurationModuleFileHandler();
        public _Advanced Advanced = new _Advanced();

        public class _Advanced
        {
            public Dictionary<string, string> GetInMemoryStorage()
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

            public SetUpFile CurrentSelection
            {
                get
                {
                    CheckAndUpdateFileHandler();
                    return AppUtility.GetFirstActiveConfig<T>();
                }
            }

            public string Selector
            {
                set
                {
                    CheckAndUpdateFileHandler();
                    AppUtility.FileHandler.Selector = value;
                }
                get
                {
                    CheckAndUpdateFileHandler();
                    return AppUtility.FileHandler.Selector;
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

            public bool RunInMemory
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
                    var activeConfig = AppUtility.GetFirstActiveConfig<T>();
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
                if (FileHandler == null) throw new Exception("FileHandler");

                AppUtility.FileHandler = FileHandler;
            }
        }

        internal T Data
        {
            get
            {
                CheckAndUpdateFileHandler();
                return AppUtility.LoadAppConfiguration<T>();
            }
        }
    }
}