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
        private static IProxyFactoryGenerator proxyFactoryGenerator;
        public static IProxyFactoryGenerator ProxyFactoryGenerator
        {
            get { return proxyFactoryGenerator; }
            set
            {
                if (proxyFactoryGenerator != null)
                {
                    throw new InvalidOperationException("ProxyFactorygenerator allready initialised");
                }
                proxyFactoryGenerator = value;
            }
        }

        private static readonly MethodInfo createGenericMethodDefinition = typeof(Container).GetMethods().AsQueryable().Where(m=>m.IsGenericMethodDefinition && m.Name == "Create").Single();

        public Container(ContainerSetup setup)
        {
            this.modelManager = setup.ModelManager;
            this.advisor = setup.Advisor;

            this.proxyFactory = ProxyFactoryGenerator.CreateProxyFactory(this);
        }

        private readonly ModelManager modelManager;
        public ModelManager ModelManager { get { return this.modelManager; } }

        private readonly Advisor advisor;
        public Advisor Advisor { get { return this.advisor; } }

        private readonly IProxyFactory proxyFactory;
        private IProxyFactory ProxyFactory { get { return proxyFactory; } }

        private readonly PropertyChangedStack pstack = new PropertyChangedStack();
        internal PropertyChangedStack PropertyChangedStack { get { return this.pstack; } }
        internal bool IsComingFrom(IInterceptedObject io, string property)
        {
            return pstack.IsComingFrom(io, property);
        }

        public bool IsInLoading { get; private set; }

        private List<InterceptedObject> loadingInstances;

        public void Load(Action loadAction)
        {
            if (this.IsInLoading)
            {
                throw new InvalidOperationException("Container is in loading state");
            }
            this.IsInLoading = true;
            this.loadingInstances = new List<InterceptedObject>();
            try
            {
                ExecuteLoading(loadAction);
            }
            finally
            {
                this.IsInLoading = false;
                this.loadingInstances = null;
            }
        }

        private void ExecuteLoading(Action loadAction)
        {
            loadAction();
            foreach(InterceptedObject instance in this.loadingInstances)
            {
                (instance as IInterceptedObject).AOPEndLoad();
            }
        }

        private T CreateInstance<T>() where T : class
        {
            T instance = this.proxyFactory.Create<T>();
            var interceptedObject = instance as InterceptedObject;
            if (interceptedObject != null)
            {
                interceptedObject.Container = this;
            }
            return instance;
        }

        public T Create<T>() where T : class
        {
            T instance = CreateInstance<T>();
            var interceptedObject = instance as InterceptedObject;
            if (interceptedObject != null)
            {
                if (this.IsInLoading)
                {
                    this.loadingInstances.Add(interceptedObject);
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
            var m = createGenericMethodDefinition.MakeGenericMethod(type);
            return m.Invoke(this, null);
        }
    }
}
