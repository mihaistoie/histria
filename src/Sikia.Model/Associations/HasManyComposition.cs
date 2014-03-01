using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class HasManyComposition<T>: HasMany<T> where T : IInterceptedObject
    {
        
    }
}
