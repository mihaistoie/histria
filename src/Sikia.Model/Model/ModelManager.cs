using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Sikia.Json;
using Sikia.Model.Helpers;
using Sikia.Sys;


namespace Sikia.Model
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
        //used only for tests
        public static ModelManager LoadModel(JsonObject config)
        {
            ModelManager model = new ModelManager();
            model.Load(config);
            return model;       
        }

        public static ModelManager LoadModelFromConfig(JsonObject config)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        ModelManager model = new ModelManager();
                        model.Load(config);
                        instance = model;
                    }
                }
            }

            return instance;
        }

        public static ModelManager Instance
        {
            get
            {
                return LoadModelFromConfig(null);
            }
        }

        public static void CleanUp()
        {
            lock (syncRoot)
            {
                instance = null;
            }

        }
        #endregion
        
        #region Model Loading

        private void LoadTypes(List<Type> types)
        {
            Type ii = typeof(IModelClass);
            Type ip = typeof(IModelPlugin);
            types.ForEach(delegate(Type iType)
            {
                if (iType.IsEnum)
                {
                    //load enums 
                    enums.Add(new EnumInfoItem(iType));
                }
                else if (iType.IsClass && ii.IsAssignableFrom(iType))
                {
                    NoModelAttribute nm = iType.GetCustomAttributes(typeof(NoModelAttribute), false).FirstOrDefault() as NoModelAttribute;
                    if (nm == null)
                    {
                        classes.Add(new ClassInfoItem(iType, false));
                    }
                }
                else if (iType.IsClass && ip.IsAssignableFrom(iType))
                {
                    classes.Add(new ClassInfoItem(iType, true));
                }

            });

        }

        private void Load(JsonObject cfg)
        {
            DateTime start = DateTime.Now;
            ModelLoader.LoadModel(cfg, this.LoadTypes);
            AfterLoad();
            TimeSpan interval = DateTime.Now - start;
            Logger.Info(Logger.MODEL, StrUtils.TT("Model loading ... done"), interval.TotalMilliseconds);

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
        ///<summary>
        /// Class by value
        ///</summary>
        public ClassInfoItem Class<T>()
        {
            return ClassByType(typeof(T));
        }
     
     
        #endregion
    }

   
}