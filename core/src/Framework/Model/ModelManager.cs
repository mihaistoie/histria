using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Sikia.Settings;
using Sikia.Aop;


namespace Sikia.Framework.Model
{

    public sealed class ModelManager
    {
        #region Private Members
        private readonly EnumCollection enums;
        private readonly ClassCollection classes;
        #endregion

        #region Singleton thread-safe pattern
        private static volatile ModelManager instance = null;
        private static object syncRoot = new Object();
        private ModelManager()
        {
            enums = new EnumCollection();
            classes = new ClassCollection();
        }
        public static ModelManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            ModelManager model = new ModelManager();
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

        private void LoadTypes(List<Type> types)
        {
            types.ForEach(delegate(Type iType)
            {
                if (iType.IsEnum)
                {
                    //load enums 
                    enums.Add(new EnumInfoItem(iType));
                }
                else if (iType.IsClass && iType.IsSubclassOf(typeof(InterceptedObject)))
                {
                    classes.Add(new ClassInfoItem(iType, false));
                }
                else if (iType.IsClass && iType.IsSubclassOf(typeof(RulePluginObject)))
                {
                    classes.Add(new ClassInfoItem(iType, true));
                }

            });

        }

        private void Load()
        {
            GlobalSettings settings = GlobalSettings.Instance();
            if ((settings.ModelTypes != null) && (settings.ModelTypes.Count() > 0))
            {
                LoadTypes(settings.ModelTypes.ToList<Type>());
            }
            else
            {
                
                Dictionary<Assembly, List<String>> nameSpaces = settings.ModelNameSpaces();
                foreach (var ns in nameSpaces)
                {
                    ns.Value.ForEach(delegate(string nameSpace)
                    {
                        LoadTypes(ns.Key.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToList<Type>());
                    });

                }
            }
            AfterLoad();

        }
        private void AfterLoad()
        {
            foreach (ClassInfoItem cc in classes)
            {
                cc.ValidateAndPrepare(this);
            }
            foreach (ClassInfoItem cc in classes)
            {
                cc.ResolveInheritance(this);
            }

            foreach (ClassInfoItem cc in classes)
            {
                cc.Loaded(this);
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
        public ClassCollection Classes { get { return classes; } }
        ///<summary>
        /// Class by type
        ///</summary>
        public ClassInfoItem ClassByType(Type ct)
        {
            try
            {
                return classes[ct];
            }
            catch
            {
                return null;
            }
        }
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