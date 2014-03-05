using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria
{
    internal interface IRoleList
    {
        void AddOrInsert(IInterceptedObject value, int index);
        void Remove(IInterceptedObject value);
    }
}
