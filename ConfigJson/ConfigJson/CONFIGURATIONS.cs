using System;
using System.Collections.Generic;
using ConfigJsonNET.ConfigurationHelper;

namespace ConfigJsonNET
{
    /// <summary>
    /// The name of the class that implements CONFIGURATION will be the name by which configuration data will be accessed
    /// eg if you have  me:CONFIGURATION then you will do me.FromSettings.UserName
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ConfigJson<T> where T : new()
    {
       // public static string ConfigLocation = @"C:\Users\";
        public static PersistedConfiguration<T> Config = new PersistedConfiguration<T>();

        #region Internal Members

        internal static string BaseDir = AppDomain.CurrentDomain.BaseDirectory;

       

        internal static string SetUpFile = @"app.config.json";
        internal static string DebugConfigFile = @"debug.config.json";
        internal static string ReleaseConfigFile = @"release.config.json";
 internal static string PathToSetupFile = BaseDir +"\\"+ SetUpFile;

        internal static List<SetUpFile> InitialSetUpFileObject = new List<SetUpFile>
        {
                new SetUpFile
                {
                    BaseDir=Config.Advanced.ConfigLocation,
                    FileName =  DebugConfigFile,
                    IsActive = true
                },
                new SetUpFile
                {
                    BaseDir=Config.Advanced.ConfigLocation,
                    FileName = ReleaseConfigFile,
                    IsActive = false
                }
            };

        #endregion
    }

    
}