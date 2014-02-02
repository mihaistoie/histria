using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sikia.Model;



namespace Sikia.Core.Castle
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
            ModelManager model = ModelProxy.Model();
            container.Register(Classes.From(model.Classes.Types).Pick().Configure(c => c.LifeStyle.Transient
                            .Interceptors(typeof(NotifyPropertyChangedInterceptor))));
        }
        #endregion
    }
}   
