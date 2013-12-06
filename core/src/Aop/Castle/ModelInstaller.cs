using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Sikia.Framework.Model;



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
            container.Register(Classes.From(ModelManager.Instance.Classes.Types).Pick().Configure(c => c.LifeStyle.Transient
                            .Interceptors(typeof(NotifyPropertyChangedInterceptor))));
        }
        #endregion
    }
}   
