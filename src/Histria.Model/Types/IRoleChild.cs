using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria
{
    interface IRoleChild
    {
        bool SetParent(IInterceptedObject value, bool updateForeignKeys);
    }
}
