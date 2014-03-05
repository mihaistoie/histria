using Castle.DynamicProxy;
using Histria.AOP;
using Histria.Core.Execution;
using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Proxy.Castle
{
    class CastleProxyFactory: IProxyFactory
    {
        private readonly ProxyGenerator generator;
        private readonly AOPInterceptor interceptor;
        private readonly List<Type> interceptedTypes;
        
        public CastleProxyFactory(Container container)
        {
            this.interceptor = new AOPInterceptor(container.Advisor);
            this.generator = new ProxyGenerator();

            //todo from model
            this.interceptedTypes = new List<Type>() {
                typeof(HasOne<>),
                typeof(HasMany<>),
                typeof(BelongsTo<>)
            };

            this.interceptedTypes.AddRange(container.ModelManager.Classes.Types);
        }

        public T Create<T>() where T:class 
        {
            //to do frameworktypes
            if (this.interceptedTypes.Contains(typeof(T)))
            {
                return this.generator.CreateClassProxy<T>(this.interceptor);
            }
            else
            {
                return Activator.CreateInstance<T>();
            }
        }
    }
}
