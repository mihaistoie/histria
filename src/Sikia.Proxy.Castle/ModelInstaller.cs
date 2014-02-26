using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sikia.Model;
using Sikia.Sys;
using System;
using System.Collections.Generic;



namespace Sikia.Proxy.Castle
{
    /// <summary>
    /// Installs all custom Models and adds on custom inteceptors to provide
    /// </summary>
    public class ModelInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Implementation
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            DateTime start = DateTime.Now;
            List<Type> frameworkTypes = new List<Type>() {
                typeof(HasOne<>),
                typeof(HasMany<>),
                typeof(BelongsTo<>)

            };
            //register Classes from Models namespace and add interceptors
            ModelManager model = ModelProxy.Model();
            container.Register(Classes.From(model.Classes.Types).Pick().Configure(c => c.LifeStyle.Transient
                            .Interceptors(typeof(NotifyPropertyChangedInterceptor))));
            //Install framework types 
            container.Register(Classes.From(frameworkTypes.ToArray()).Pick().Configure(c => c.LifeStyle.Transient
                                                    .Interceptors(typeof(NotifyPropertyChangedInterceptor))));
            
            TimeSpan interval = DateTime.Now - start;
            Logger.Info(Logger.MODEL, StrUtils.TT("Install castle proxy ... done"), interval.TotalMilliseconds);
        }
        #endregion
    }
}
