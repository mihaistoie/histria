using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Sikia.Core.Execution;

namespace Sikia.Proxy.Castle
{
    public class CastleFactory: IProxyFactoryGenerator
    {
        public static void Install()
        {
            Container.ProxyFactoryGenerator = new CastleFactory();
        }

        public IProxyFactory CreateProxyFactory(Container container)
        {
            return new CastleProxyFactory(container);
        }
    }
}
