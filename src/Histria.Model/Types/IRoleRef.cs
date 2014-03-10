using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria
{
    internal interface IRoleRef
    {
        void SetValue(IInterceptedObject value);
        IInterceptedObject GetValue();
    }
}
