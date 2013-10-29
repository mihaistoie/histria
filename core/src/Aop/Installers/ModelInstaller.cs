using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sikia.Models;
using Sikia.Framework;



namespace Sikia
{
    /// <summary>
    /// Installs all custom Models and adds on custom inteceptors to provide
    /// </summary>
    public class ModelInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Implementation
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //register Classes fram Models namespace and add interceptors
            container.Register(Classes.FromThisAssembly().InSameNamespaceAs<DummyClass>(true).Configure(c => c.LifeStyle.Transient
                            .Interceptors(typeof(NotifyPropertyChangedInterceptor))));
        }
        #endregion
    }
}   
