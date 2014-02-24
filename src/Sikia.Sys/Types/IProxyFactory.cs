using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia
{
    public interface IProxyFactory
    {
        T Resolve<T>(); 
    }
}
