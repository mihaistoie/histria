using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Histria.Settings;
using Histria.Model;

namespace Histria.Application
{
    ///<summary>
    /// Global Application used to initialize / uninitialize une application
    ///</summary>
    static public class GlbApplicaton
    {
        public static void Stop()
        {
        }
        public static void Start()
        {
            // Load application settings  
            GlobalSettings settings = GlobalSettings.Instance;
            // Load current model
            ModelManager model = ModelManager.LoadModelFromConfig(settings.Model()); 
        }

    }
}
