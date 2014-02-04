using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Sikia.Proxy.Castle
{
    /// <summary>
    /// Installs all castle Interceptors
    /// </summary>
    public class InterceptorInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Implementation

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<IInterceptor>());
        }

        #endregion
    }
}
