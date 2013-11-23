using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Sikia.Settings;
using Sikia.Aop;

namespace Sikia.Framework.Model
{

    public sealed class Model
    {
        #region Private Members
        private readonly EnumCollection enums;
        private readonly ClassCollection classes;
        #endregion

        #region Singleton thread-safe pattern
        private static volatile Model instance = null;
        private static object syncRoot = new Object();
        private Model()
        {
            enums = new EnumCollection();
            classes = new ClassCollection();
        }
        public static Model Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            Model model = new Model();
                            model.Load();
                            instance = model;
                        }
                    }
                }

                return instance;
            }
        }
        #endregion
        #region Implementation
        #region Model Loading

        private void Load()
        {
            GlobalSettings settings = GlobalSettings.Instance;
            Dictionary<Assembly, List<String>> nameSpaces = settings.ModelNameSpaces();
            foreach (var ns in nameSpaces)
            {
                ns.Value.ForEach(delegate(string nameSpace)
                {
                    List<Type> allTypes = ns.Key.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToList<Type>();
                    allTypes.ForEach(delegate(Type iType)
                    {
                        if (iType.IsEnum)
                        {
                            //load enums 
                            enums.Add(new EnumInfoItem(iType));
                        }
                        else if (iType.IsClass && iType.IsSubclassOf(typeof(InterceptedObject)))
                        {
                            classes.Add(new ClassInfoItem(iType));
                        }

                    });

                });
                AfterLoad();
            }

        }
        private void AfterLoad()
        {
            foreach (ClassInfoItem cc in classes)
            {
                cc.AfterLoad();
            }
        }
        #endregion
        #endregion


        #region Properties
        ///<summary>
        /// List of enums used by Application 
        ///</summary>
        public EnumCollection Enums { get { return enums; } }

        ///<summary>
        /// List of Model Classes used by Application 
        ///</summary>
        public ClassCollection ModelClasses { get { return classes; } }
        #endregion
    }

    public class EnumCollection : KeyedCollection<Type, EnumInfoItem>
    {
        protected override Type GetKeyForItem(EnumInfoItem item)
        {
            return item.EnumType;
        }
    }
    public class ClassCollection : KeyedCollection<Type, ClassInfoItem>
    {
        protected override Type GetKeyForItem(ClassInfoItem item)
        {
            return item.ClassTypeInfo;
        }
        public Type[] Types
        {
            get
            {
                if (this.Dictionary != null)
                {
                    return this.Dictionary.Keys.ToArray<Type>();
                }
                else
                {
                    return this.Select(i => this.GetKeyForItem(i)).ToArray();
                }
            }
        }
    }
}