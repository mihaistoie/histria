using Sikia.Core;
using Sikia.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Proxy.Castle
{
    public class ModuleCastle : ModulePlugIn
    {
        public override void Register(params object[] args)
        {
            CastleFactory.Install();
            ProxyFactory.Instance.Factory = CastleFactory.Instance as IProxyFactory;
        }
    }
}

