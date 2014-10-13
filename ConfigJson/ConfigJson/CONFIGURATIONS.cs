using ConfigJsonNET.ConfigurationHelper;
using System;
using System.Collections.Generic;

namespace ConfigJsonNET
{
    /// <summary>
    /// The name of the class that implements CONFIGURATION will be the name by which configuration data will be accessed
    /// eg if you have  me:CONFIGURATION then you will do me.FromSettings.UserName
    /// </summary>

    public abstract class ConfigJson<T> where T : new()
    {
        // public static string ConfigLocation = @"C:\Users\";
        public static PersistedConfiguration<T> Config = new PersistedConfiguration<T>();

        #region Internal Members

        internal static string BaseDir = AppDomain.CurrentDomain.BaseDirectory;

        internal static string SetUpFile = @"app.config." + typeof(T).FullName + ".json";
        internal static string DebugConfigFile = @"debug.config." + typeof(T).FullName + ".json";
        internal static string ReleaseConfigFile = @"release.config." + typeof(T).FullName + ".json";
        internal static string PathToSetupFile = BaseDir + "\\" + SetUpFile;
       

        internal static List<SetUpFile> InitialSetUpFileObject = new List<SetUpFile>
        {
                new SetUpFile
                {
                    BaseDir=Config.Advanced.ConfigLocation,
                    FileName =  DebugConfigFile,
                    IsActive = true,
                    Selector = "debug"
                },
                new SetUpFile
                {
                    BaseDir=Config.Advanced.ConfigLocation,
                    FileName = ReleaseConfigFile,
                    IsActive = false,
                    Selector = "release"
                }
            };

        #endregion Internal Members

        public static T Data
        {
            get { return Config.Data; }
        }
    }
}