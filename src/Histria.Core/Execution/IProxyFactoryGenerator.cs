﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Execution
{
    public interface IProxyFactoryGenerator
    {
        IProxyFactory CreateProxyFactory(Container container);
    }
}
