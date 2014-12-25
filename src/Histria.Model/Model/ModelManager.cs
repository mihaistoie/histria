using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Histria.Json;
using Histria.Model.Helpers;
using Histria.Sys;


namespace Histria.Model
{

    public sealed class ModelManager
    {
        #region Private Members
        private readonly EnumCollection enums;
        private readonly ClassCollection viewsandclasses;
        private readonly ClassCollection classes;
        private readonly ClassCollection views;

        private static readonly IList<Type> frameworkTypes = new Type[]
        {
            typeof(PropertyState)
        };

        #endregion

        private ModelManager()
        {
            this.enums = new EnumCollection();
            this.views = new ClassCollection();
            this.classes = new ClassCollection();
            this.viewsandclasses = new ClassCollection();
        }
        //used only for tests
        public static ModelManager LoadModel(JsonObject config)
        {
            ModelManager model = new ModelManager();
            model.Load(config);
            return model;
        }

        #region Model Loading

        private void LoadTypes(List<Type> types)
        {
            Type ii = typeof(IClassModel);
            Type iw = typeof(IViewModel);
            Type ip = typeof(IPluginModel);
            for (int i = 0, len = types.Count; i < len; i++)
            {
                Type iType = types[i];
                ClassInfoItem ci;
                NoModelAttribute nm = iType.GetCustomAttributes(typeof(NoModelAttribute), false).FirstOrDefault() as NoModelAttribute;
                if (nm != null) continue;

                if (iType.IsEnum)
                {
                    //load enums 
                    this.enums.Add(new EnumInfoItem(iType));
                }
                else if (iType.IsClass)
                {
                    if (iw.IsAssignableFrom(iType))
                    {
                        ci = new ViewInfoItem(iType);
                        this.views.Add(ci);
                        this.viewsandclasses.Add(ci);
                    }
                    else if (ii.IsAssignableFrom(iType))
                    {
                        ci = new ClassInfoItem(iType, false);
                        this.classes.Add(ci);
                        this.viewsandclasses.Add(ci);
                    }
                    else if (ip.IsAssignableFrom(iType))
                    {
                        ci = new ClassInfoItem(iType, true);
                        this.classes.Add(ci);
                        this.viewsandclasses.Add(ci);
                    }
                }

            }

        }

        private void Load(JsonObject cfg)
        {
            DateTime start = DateTime.Now;
            ModelLoader.LoadModel(cfg, this.LoadTypes);
            AfterLoad();
            TimeSpan interval = DateTime.Now - start;
            Logger.Info(Logger.MODEL, L.T("Model loading ... done"), interval.TotalMilliseconds);

        }

        private void AfterLoad()
        {
            foreach (ClassInfoItem cc in viewsandclasses)
            {
                cc.ValidateAndPrepare(this);
            }
            foreach (ClassInfoItem cc in viewsandclasses)
            {
                cc.ResolveInheritance(this);
            }

            foreach (ClassInfoItem cc in viewsandclasses)
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
        /// List of View Model used by Application 
        ///</summary>
        public ClassCollection Views { get { return views; } }

        ///<summary>
        /// List of Views and Classes used by Application 
        ///</summary>
        public ClassCollection ViewsAndClasses { get { return viewsandclasses; } }
        ///<summary>
        /// Class by type
        ///</summary>
        internal ClassInfoItem ClassByType(Type ct)
        {
            ClassInfoItem result;
            viewsandclasses.TryGetClassInfo(ct, out result);
            return result;
        }
        ///<summary>
        /// Class by value
        ///</summary>
        public ClassInfoItem Class<T>()
        {
            return ClassByType(typeof(T));
        }

        public IList<Type> GetAOPInterceptedTypes()
        {
            List<Type> interceptedTypes = new List<Type>(frameworkTypes);
            //TODO views?
            interceptedTypes.AddRange(this.ViewsAndClasses.Types);
            return interceptedTypes;
        }
        #endregion
    }


}