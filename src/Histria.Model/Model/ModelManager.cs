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
        private readonly bool _checkModel;
        private readonly EnumCollection _enums;
        private readonly ClassCollection _viewsandclasses;
        private readonly ClassCollection _classes;
        private readonly ClassCollection _views;

        private static readonly IList<Type> _frameworkTypes = new Type[]
        {
            typeof(PropertyState)
        };

        #endregion

        private ModelManager()
        {
            this._enums = new EnumCollection();
            this._views = new ClassCollection();
            this._classes = new ClassCollection();
            this._viewsandclasses = new ClassCollection();
            this._checkModel = true;
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

            //Load enums first
            types.FindAll(x => x.IsEnum).ToList().ForEach(x =>
            {
                this._enums.Add(new EnumInfoItem(x));
            });

            for (int i = 0, len = types.Count; i < len; i++)
            {
                Type iType = types[i];
                ClassInfoItem ci;
                NoModelAttribute nm = iType.GetCustomAttributes(typeof(NoModelAttribute), false).FirstOrDefault() as NoModelAttribute;
                if (nm != null) continue;

                if (iType.IsClass)
                {
                    if (iw.IsAssignableFrom(iType))
                    {
                        ci = new ViewInfoItem(iType);
                        this._views.Add(ci);
                        this._viewsandclasses.Add(ci);
                    }
                    else if (ii.IsAssignableFrom(iType))
                    {
                        ci = new ClassInfoItem(iType, false);
                        this._classes.Add(ci);
                        this._viewsandclasses.Add(ci);
                    }
                    else if (ip.IsAssignableFrom(iType))
                    {
                        ci = new ClassInfoItem(iType, true);
                        this._classes.Add(ci);
                        this._viewsandclasses.Add(ci);
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
            foreach (ClassInfoItem cc in _viewsandclasses)
            {
                cc.ValidateAndPrepare(this);
            }
            foreach (ClassInfoItem cc in _viewsandclasses)
            {
                cc.ResolveInheritance(this);
            }

            foreach (ClassInfoItem cc in _viewsandclasses)
            {
                cc.Loaded(this);
            }
        }

        #endregion

        #region Properties

        ///<summary>
        /// Check model consistency during loading
        ///</summary>
        public bool CheckModel { get { return _checkModel; } }

        ///<summary>
        /// List of enums used by Application 
        ///</summary>
        public EnumCollection Enums { get { return _enums; } }

        ///<summary>
        /// List of Model Classes used by Application 
        ///</summary>
        public ClassCollection Classes { get { return _classes; } }

        ///<summary>
        /// List of View Model used by Application 
        ///</summary>
        public ClassCollection Views { get { return _views; } }

        ///<summary>
        /// List of Views and Classes used by Application 
        ///</summary>
        public ClassCollection ViewsAndClasses { get { return _viewsandclasses; } }
        ///<summary>
        /// Class by type
        ///</summary>
        internal ClassInfoItem ClassByType(Type ct)
        {
            ClassInfoItem result;
            _viewsandclasses.TryGetClassInfo(ct, out result);
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
            List<Type> interceptedTypes = new List<Type>(_frameworkTypes);
            //TODO views?
            interceptedTypes.AddRange(this.ViewsAndClasses.Types);
            return interceptedTypes;
        }
        #endregion
    }


}