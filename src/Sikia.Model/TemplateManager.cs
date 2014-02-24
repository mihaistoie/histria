using Sikia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    ///<summary>
    /// Used to register an attribute 
    ///</summary>  
    public sealed class TemplateManager
    {
        #region Private Members
        private readonly Dictionary<string, TemplateAttribute> templates;
        #endregion

        #region Singleton thread-safe pattern
        private static volatile TemplateManager instance = null;
        private static object syncRoot = new Object();
        private TemplateManager()
        {
            templates = new Dictionary<string, TemplateAttribute>();
        }

        public static TemplateManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            TemplateManager tm = new TemplateManager();
                            instance = tm;
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        /*public static void Register(string templateName, TemplateAttribute template,  Type templateType)  
        {

        }*/
    }
}
