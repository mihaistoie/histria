using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core.Execution
{
    public interface IProxyFactoryGenerator
    {
        IProxyFactory CreateProxyFactory(Container container);
    }
}
