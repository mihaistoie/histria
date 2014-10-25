using Histria.AOP;
using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Histria.Core.Execution
{
    public class Container
    {
        private static IProxyFactoryGenerator _proxyFactoryGenerator;
        public static IProxyFactoryGenerator ProxyFactoryGenerator
        {
            get { return _proxyFactoryGenerator; }
            set
            {
                if (_proxyFactoryGenerator != null)
                {
                    throw new InvalidOperationException("ProxyFactorygenerator allready initialised");
                }
                _proxyFactoryGenerator = value;
            }
        }

        private static readonly MethodInfo _createGenericMethodDefinition = typeof(Container).GetMethods().AsQueryable().Where(m=>m.IsGenericMethodDefinition && m.Name == "Create").Single();

        public Container(ContainerSetup setup)
        {
            this._modelManager = setup.ModelManager;
            this._advisor = setup.Advisor;
            this._proxyFactory = ProxyFactoryGenerator.CreateProxyFactory(this);
        }


        private readonly ModelManager _modelManager;
        /// <summary>
        /// Model manager
        /// </summary>
        public ModelManager ModelManager { get { return this._modelManager; } }

        private readonly Advisor _advisor;
        /// <summary>
        /// Advisor
        /// </summary>
        public Advisor Advisor { get { return this._advisor; } }

        private readonly IProxyFactory _proxyFactory;
        private IProxyFactory ProxyFactory { get { return _proxyFactory; } }

        private readonly PropertyChangedStack _pstack = new PropertyChangedStack();
        
        internal PropertyChangedStack PropertyChangedStack { get { return this._pstack; } }
        internal bool IsComingFrom(IInterceptedObject io, string property)
        {
            return _pstack.IsComingFrom(io, property);
        }

        private int _loadingCallCount = 0;
        private List<InterceptedObject> _loadingInstances;

        #region Private Methods
        private bool IsInLoading { get{return _loadingCallCount != 0;}}

        /// <summary>
        /// Satart loading from persitance
        /// </summary>
        private void StartLoading()
        {
            if (this._loadingCallCount == 0 && this._loadingInstances == null)
            {
                this._loadingInstances = new List<InterceptedObject>();
            }
            this._loadingCallCount++;
        }

        /// <summary>
        /// After loading call  rules "AfterLoading"
        /// </summary>
        private void EndLoading()
        {
            if (this._loadingCallCount <= 0)
            {
                throw new InvalidOperationException("Loading count error");
            }
            this._loadingCallCount--;
            if (this._loadingCallCount == 0)
            {
                foreach (InterceptedObject instance in this._loadingInstances)
                {
                    (instance as IInterceptedObject).AOPEndLoad();
                }
                this._loadingInstances.Clear();
            }
        }

        /// <summary>
        ///  Create an instance of intercepted object and add it to container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T CreateInstance<T>() where T : class
        {
            T instance = this._proxyFactory.Create<T>();
            var interceptedObject = instance as InterceptedObject;
            if (interceptedObject != null)
            {
                interceptedObject.Container = this;
            }
            return instance;
        }

        #endregion

        /// <summary>
        /// Load one or many instances from persistence
        /// </summary>
        /// <param name="loadAction"></param>
        public void Load(Action loadAction)
        {
            this.StartLoading();
            try
            {
                loadAction();
            }
            finally
            {
                this.EndLoading();
            }
        }

        /// <summary>
        ///  Create an instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Create<T>() where T : class
        {
            T instance = CreateInstance<T>();
            var interceptedObject = instance as InterceptedObject;
            if (interceptedObject != null)
            {
                if (this.IsInLoading)
                {
                    this._loadingInstances.Add(interceptedObject);
                    (interceptedObject as IInterceptedObject).AOPBeginLoad();
                }
                else
                {
                    (interceptedObject as IInterceptedObject).AOPCreate();
                }
            }
            return instance;
        }

        public object Create(Type type)
        {
            var m = _createGenericMethodDefinition.MakeGenericMethod(type);
            return m.Invoke(this, null);
        }


        //To review
        public V CreateView<V,T>(T model) 
            where T: InterceptedObject
            where V: ViewObject<T>
        {

            V view = null;
            this.Load(() =>
                {
                    view = this.Create<V>();
                    view.Init((T)model);
                });
            return view;
        }
        //To review
        public void DereferenceView(ViewObject view)
        {
            InterceptedObject model = view.GetModel();
            if (model == null)
            {
                return;
            }
            if (model.Views.RemoveRef(view) <= 0)
            {
                view.CleanObject();
            }
        }
    }
}
