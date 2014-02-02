using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sikia.Sys
{
    public abstract class ModulePlugIn
    {
        public abstract void Register();
        private static void RegisterModule(Type type)
        {
            ModulePlugIn plugIn = (ModulePlugIn)Activator.CreateInstance(type);
            plugIn.Register();
        }
        private static string AssemblyDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }
        public static void Load(string module)
        {
            try
            {
                var fileName = Path.Combine(ModulePlugIn.AssemblyDirectory, module + ".dll");
                if (File.Exists(fileName))
                {

                    var basePlugin = typeof(ModulePlugIn);
                    var pluginDll = Assembly.LoadFile(fileName);
                    foreach (Type type in pluginDll.GetExportedTypes())
                    {
                        if (type.IsSubclassOf(basePlugin))
                        {
                            ModulePlugIn.RegisterModule(type);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(Logger.PLUGIN, ex);
            }
        }


    }

}
