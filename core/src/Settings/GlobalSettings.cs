using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;


namespace Sikia.Settings
{
    ///<summary>
    /// Application global settings (singleton)
    ///</summary>
    public class GlobalSettings
    {
        private readonly List<String> modelNameSpaces = new List<String>();
        public Type[] ModelTypes = null;

        ///<summary>
        /// Load settings from  a JSON config file
        ///</summary>
        private void Load(ApplicationConfig config)
        {
            //TODO: Load settings from  a JSON config file
            ModelTypes = config.Types;
            modelNameSpaces.AddRange(config.Namespaces.ToList<string>());
        }

        #region Singleton thread-safe pattern 
        private static volatile GlobalSettings instance = null;
        private static object syncRoot = new Object();
        private GlobalSettings()
        {
        }
        public static GlobalSettings Instance()
        {
            return Instance(null);
        }
        public static GlobalSettings Instance(ApplicationConfig config)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        GlobalSettings settings = new GlobalSettings();
                        if (config == null)
                            config = new ApplicationConfig();
                        settings.Load(config);
                        instance = settings;
                    }
                }
            }

            return instance;
        }
        #endregion

        #region Implementation details

        private string matchNameSpace(string nameSpace)
        {
            if (String.IsNullOrEmpty(nameSpace))
            {
                return "";
            }
            foreach (string ns in modelNameSpaces)
            {
                string fns = "." + ns + ".";
                int ii = nameSpace.IndexOf(fns);
                if (ii > 0)
                {
                    return nameSpace.Substring(0, ii) + "." + ns;
                }
                fns = "." + ns;
                if (nameSpace.EndsWith(fns))
                {
                    return nameSpace.Substring(0, nameSpace.Length - fns.Length) + "." + ns;
                }
                fns = ns + ".";
                if (nameSpace.StartsWith(fns))
                {
                    return ns;
                }
            }
            return "";
        }

        private List<String> nameSpacesForAssembly(Assembly asm)
        {
            List<String> res = null;
            GlobalSettings self = this;
            var allNameSpaces = asm.GetTypes().Select(t => t.Namespace)
                .Distinct().Select(t => self.matchNameSpace(t)).Distinct();
            
            foreach (string nameSpace in allNameSpaces)
            {
                if (!String.IsNullOrEmpty(nameSpace))
                {
                    if (res == null)
                        res = new List<String>();
                    res.Add(nameSpace);
                }
            }
            return res;
        }

        private void processAssembly(Assembly asm, Dictionary<Assembly, List<String>> result)
        {
            List<String> nsfa = this.nameSpacesForAssembly(asm);
            if (nsfa != null)
            {
                result[asm] = nsfa;
            }
        }

        #endregion

        #region Public Properties && Methods
        ///<summary>
        /// List of namespaces that contains Model "Classes" 
        ///</summary>
        public Dictionary<Assembly, List<String>> ModelNameSpaces()
        {
            Dictionary<Assembly, List<String>> res = new Dictionary<Assembly, List<String>>();
            processAssembly(Assembly.GetExecutingAssembly(), res);
            foreach (AssemblyName an in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                processAssembly(Assembly.Load(an.ToString()), res);
            }
            return res;
        }
        #endregion

    }

}
