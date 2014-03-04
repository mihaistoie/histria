using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia
{
    internal interface IRoleList
    {
        void AddOrInsert(IInterceptedObject value, int index);
        void Remove(IInterceptedObject value);
    }
}
