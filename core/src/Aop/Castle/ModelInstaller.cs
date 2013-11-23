using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sikia.Models;
using Sikia.Framework;
using Sikia.Framework.Model;
using System.Collections.Generic;



namespace Sikia.Aop.Castle
{
    /// <summary>
    /// Installs all custom Models and adds on custom inteceptors to provide
    /// </summary>
    public class ModelInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Implementation
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //register Classes from Models namespace and add interceptors
            container.Register(Classes.From(Model.Instance.ModelClasses.Types).Pick().Configure(c => c.LifeStyle.Transient
                            .Interceptors(typeof(NotifyPropertyChangedInterceptor))));
        }
        #endregion
    }
}   
