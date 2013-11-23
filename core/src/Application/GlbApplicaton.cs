﻿using Sikia.Framework.Model;
using Sikia.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Application
{
    ///<summary>
    /// Global Application used to initialize / uninitialize une application
    ///</summary>
    static class GlbApplicaton
    {
        public static void Start()
        {
            // Load application settings  
            GlobalSettings settings = GlobalSettings.Instance;
            // Load current model
            Model model = Model.Instance;

        }

    }
}
