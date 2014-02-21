using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class HasOne<T> : Association<T>, IRoleRef where T : IInterceptedObject
    {
        private IInterceptedObject value;
    }
}


