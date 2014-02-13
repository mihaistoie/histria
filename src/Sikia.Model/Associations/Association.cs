using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public abstract class Association<T> : IAssociation where T : IInterceptedObject
    {
    }
}
