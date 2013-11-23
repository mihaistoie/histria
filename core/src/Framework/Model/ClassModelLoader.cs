using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sikia.Aop;
using Sikia.Framework.Attributes;

namespace Sikia.Framework.Model
{
    class ClassModelLoader
    {
        public static void LoadFromNameSpace(string nameSpace, Model model)
        {
            Assembly cAssembly = Assembly.GetExecutingAssembly();
            List<Type> allTypes = cAssembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToList<Type>();
            allTypes.ForEach(delegate(Type iType)
            {
                if (iType.IsEnum)
                {
                    //load enums 
                    model.Enums.Add(new EnumInfoItem(iType));
                }
                else if (iType.IsClass && iType.IsSubclassOf(typeof(InterceptedObject)))
                {
                    model.ModelClasses.Add(new ClassInfoItem(iType));
                }
                    
                
            });


        }

    }
}



/*
    static class ModelLoader
    {
        public static void ReadAttributes(string nameSpace)
        {
            Assembly cAssembly = Assembly.GetExecutingAssembly();
            Type[] types = cAssembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
            foreach (Type type in types)
            {
                if (type.IsEnum)
                {
                    DisplayAttribute da = type.GetAttributeValue((DisplayAttribute dd) => dd);
                    if (da != null)
                    {
                        System.Console.WriteLine("Enum \"{0}\" (\"{1}\")", da.Title, da.Description);
                    }
                    Array values = Enum.GetValues(type);
                    EnumCaptions ec = type.GetAttributeValue((EnumCaptions ee) => ee);
                    if (ec != null) {
                        if (values.Length != ec.Titres.Count) {
                            //Model error  
                        } else {
                            int i = 0;
                            foreach(var v in values) {
                                System.Console.WriteLine("{0} {1} -> {2} ({3})", "\t", (int)v, v.ToString(), ec.Titres[i]);
                                i++;
                            }
                        }
                    }
                }
                else
                {
                    DisplayAttribute da = type.GetAttributeValue((DisplayAttribute dd) => dd);
                    if (da != null)
                    {
                        System.Console.WriteLine("Description of \"{0}\"  is \"{1}\"", da.Title, da.Description);
                    }
                    
                    
                }
            }
        }
    }
} */