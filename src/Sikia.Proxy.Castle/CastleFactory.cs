using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Sikia.Proxy.Castle
{
    public class CastleFactory: IProxyFactory
    {

        private WindsorContainer container = null;

        #region Singleton thread-safe pattern
        private static volatile CastleFactory instance = null;
        private static object syncRoot = new Object();
        private CastleFactory()
        {
            container = new WindsorContainer();
            container.Install(FromAssembly.This());
        }
        public static CastleFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            CastleFactory cf = new CastleFactory();
                            instance = cf;
                        }
                    }
                }

                return instance;
            }
        }
        #endregion
        
        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }
        public static void Install()
        {
            var inst = CastleFactory.Instance;
        }
    }
}
