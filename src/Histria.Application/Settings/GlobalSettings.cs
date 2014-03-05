using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Histria.Json;


namespace Histria.Settings
{
    ///<summary>
    /// Application global settings (singleton)
    ///</summary>
    public class GlobalSettings
    {
        private JsonObject settings = null;
   
        ///<summary>
        /// Load settings from  a JSON config file
        ///</summary>
        private void Load(JsonObject config)
        {
            settings = config;
        }
        #region Singleton thread-safe pattern 
        private static volatile GlobalSettings instance = null;
        private static object syncRoot = new Object();
        private GlobalSettings()
        {
        }
  
        public static void CleanUp()
        {
            lock (syncRoot)
            {
                instance = null;
            }

        }

        public static GlobalSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            GlobalSettings settings = new GlobalSettings();
                            settings.Load(null);
                            instance = settings;
                        }
                    }
                }

                return instance;
            }
        }

        public JsonObject Model()
        {
            return (JsonObject)JsonValue.Parse("{\"nameSpaces\": [\"Models\"]}");
        }
        #endregion

  
    }

}
