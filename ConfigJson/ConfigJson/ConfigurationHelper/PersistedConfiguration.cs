using System;
using System.Collections.Generic;

namespace ConfigJsonNET.ConfigurationHelper
{
    public class PersistedConfiguration<T> where T : new()
    {
        public PersistedConfiguration()
        {
            CheckAndUpdateFileHandler();
        }

        internal static string _ConfigLocation = @"C:\Users\";

        public T InitialConfigurationLoaded { set; get; }

        public static IConfigurationModuleFileHandler FileHandler = new ConfigurationModuleFileHandler<T>();
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
                    //   CheckAndUpdateFileHandler();
                    return AppUtility.GetFirstActiveConfig<T>();
                }
            }


           public string   SetUpFile
            {
               
                get
                {
                    //  CheckAndUpdateFileHandler();
                    return AppUtility.FileHandler.SetUpFiles[typeof(T).FullName];
                }
            }

            public string Selector
            {
                set
                {
                    //  CheckAndUpdateFileHandler();

                    AppUtility.FileHandler.Selector[typeof(T).FullName] = value;
                }
                get
                {
                    //  CheckAndUpdateFileHandler();
                    return AppUtility.FileHandler.Selector[typeof(T).FullName];
                }
            }

            public bool AllowOverwrite
            {
                set
                {
                    // CheckAndUpdateFileHandler();
                    AppUtility.FileHandler.AllowOverwrite[typeof(T).FullName] = value;
                }
                get
                {
                    // CheckAndUpdateFileHandler();
                    return AppUtility.FileHandler.AllowOverwrite[typeof(T).FullName];
                }
            }

            public bool RunInMemory
            {
                set
                {
                    //   CheckAndUpdateFileHandler();
                    AppUtility.FileHandler.RunInMemory[typeof(T).FullName] = value;
                }
                get
                {
                    //  CheckAndUpdateFileHandler();
                    return AppUtility.FileHandler.RunInMemory[typeof(T).FullName];
                }
            }

            public T PersistedData
            {
                set
                {
                    //   CheckAndUpdateFileHandler();
                    var activeConfig = AppUtility.GetFirstActiveConfig<T>();
                    var activeFileName = activeConfig.FileName;
                    var baseDir = activeConfig.BaseDir;

                    AppUtility.Persist(AppUtility.CopyObject(value), baseDir + activeFileName, typeof(T).FullName);
                }
            }
        }

        internal T Data
        {
            get
            {
                //  CheckAndUpdateFileHandler();
                return AppUtility.LoadAppConfiguration<T>();
            }
        }

        private static void CheckAndUpdateFileHandler()
        {
            if (AppUtility.FileHandler == null)
            {
                if (FileHandler == null) throw new Exception("FileHandler");

                AppUtility.FileHandler = FileHandler;
            }

            AppUtility.FileHandler.RunInMemory = AppUtility.FileHandler.RunInMemory ??
                                                               new Dictionary<string, bool>();

            AppUtility.FileHandler.AllowOverwrite = AppUtility.FileHandler.AllowOverwrite ?? new Dictionary<string, bool>();

            AppUtility.FileHandler.Selector = AppUtility.FileHandler.Selector ?? new Dictionary<string, string>();
            AppUtility.FileHandler.SetUpFiles = AppUtility.FileHandler.SetUpFiles ?? new Dictionary<string, string>();
            if (!AppUtility.FileHandler.RunInMemory.ContainsKey(typeof(T).FullName))
            {
                AppUtility.FileHandler.RunInMemory[typeof(T).FullName] = false;
            }

            if (!AppUtility.FileHandler.AllowOverwrite.ContainsKey(typeof(T).FullName))
            {
                AppUtility.FileHandler.AllowOverwrite[typeof(T).FullName] = false;
            }

            if (!AppUtility.FileHandler.Selector.ContainsKey(typeof(T).FullName))
            {
                AppUtility.FileHandler.Selector[typeof(T).FullName] = null;
            }

            if (!AppUtility.FileHandler.SetUpFiles.ContainsKey(typeof(T).FullName))
            {
                AppUtility.FileHandler.SetUpFiles[typeof(T).FullName] = null;
            }
        }
    }
}