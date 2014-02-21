using Sikia.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sikia.Model.Helpers
{
    public delegate void LoadTypes(List<Type> ModelTypes);

    internal static class ModelLoader
    {
        ///<summary>
        /// Load model. Load all classes described by cfg 
        ///</summary>   
        
        // cfg = {"types": ["Customer", "Address", "Country"]}  //list of classes to load
        // or 
        // cfg = {"nameSpaces": ["Model.CRM","Model.Library"]}  //list namespaces (segments of namespaces)  to scan
        static public void LoadModel(JsonObject cfg, LoadTypes lt)
        {
            Assembly[] used = AppDomain.CurrentDomain.GetAssemblies();
            List<Assembly> list = new List<Assembly>();
            foreach (Assembly a in used)
            {
                if (!isSystem(a.FullName))
                {
                    list.Add(a);
                }
            }
 
            if (cfg.Keys.Contains("types"))
            {
               
                JsonArray ja = (JsonArray)cfg["types"];
                List<Type> mTypes = new List<Type>();

                foreach (JsonValue v in ja)
                {
                    string s = v;
                    foreach (Assembly a in list)
                    {
                        Type tt = a.GetType(s);
                        if (tt != null)
                        {
                            mTypes.Add(tt);
                            break;
                        }

                    }
                  
                }
                lt(mTypes);

            }
            else if (cfg.Keys.Contains("nameSpaces"))
            {
                JsonArray ja = (JsonArray)cfg["nameSpaces"];
                List<string> namespaces = new List<string>();
                foreach (JsonValue v in ja)
                {
                    namespaces.Add(v);
                }
                Dictionary<Assembly, List<String>> nss = getModelNameSpaces(namespaces, list);
                foreach (var ns in nss)
                {
                    ns.Value.ForEach(delegate(string nameSpace)
                    {
                        lt(ns.Key.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToList<Type>());
                    });

                }

            }

        }

        #region Implementation details

        private static bool isSystem(string name)
        {
            return name.StartsWith("System.") || name.StartsWith("Microsoft.")
                || name.StartsWith("System,") || name.StartsWith("mscorlib,");
        }
      
        private static string matchNameSpace(string nameSpace, List<String> modelNameSpaces)
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

        private static List<String> nameSpacesForAssembly(Assembly asm, List<String> modelNameSpaces)
        {
            List<String> res = null;
            var allNameSpaces = asm.GetTypes().Select(t => t.Namespace)
                .Distinct().Select(t => ModelLoader.matchNameSpace(t, modelNameSpaces)).Distinct();

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

        private static void processAssembly(Assembly asm, List<String> modelNameSpaces, Dictionary<Assembly, List<String>> result)
        {
            List<String> nsfa = ModelLoader.nameSpacesForAssembly(asm, modelNameSpaces);
            if (nsfa != null)
            {
                result[asm] = nsfa;
            }
        }

        private static Dictionary<Assembly, List<String>> getModelNameSpaces(List<String> strNamespaces, List<Assembly> assemblies)
        {
            Dictionary<Assembly, List<String>> res = new Dictionary<Assembly, List<String>>();
            foreach (Assembly a in assemblies)
            {
                processAssembly(a, strNamespaces, res);
            }
            return res;
        }

        #endregion

    }
}
