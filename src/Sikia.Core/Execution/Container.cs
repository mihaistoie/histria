using Sikia.AOP;
using Sikia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core.Execution
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

        public T Load<T>(Action<T> loadAction) where T:class
        {
            T instance = this.CreateInstance<T>();
            var interceptedObject = instance as InterceptedObject;
            if (interceptedObject != null)
            {
                (interceptedObject as IInterceptedObject).AOPLoad(loadAction);
            }
            return instance;
        }

        public T Create<T>() where T : class
        {
            T instance = CreateInstance<T>();
            var interceptedObject = instance as InterceptedObject;
            if (interceptedObject != null)
            {
                (interceptedObject as IInterceptedObject).AOPCreate();
            }
            return instance;
        }
    }
}
