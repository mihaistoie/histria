using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Settings;
using Sikia.Framework.Model;
using Sikia.Aop;
using Sikia.DataBase;

namespace Sikia.Application
{
    ///<summary>
    /// Global Application used to initialize / uninitialize une application
    ///</summary>
    static public class GlbApplicaton
    {
        public static void Start()
        {
            GlbApplicaton.Start(null);
        }
        public static void Stop()
        {
            //uninstall 
            ModelManager.CleanUp();
            DbConnectionManger.CleanUp();
            GlobalSettings.CleanUp();
        }
        public static void Start(ApplicationConfig config)
        {
            // Load application settings  
            GlobalSettings settings = GlobalSettings.Instance(config);
            // Load Connection manager
            DbConnectionManger db = DbConnectionManger.Instance(config);
            // Load current model
            ModelManager model = ModelManager.Instance;

        }

    }
}
