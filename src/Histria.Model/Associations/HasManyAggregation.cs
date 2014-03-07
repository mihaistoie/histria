using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public class HasManyAggregation<T> : HasMany<T> where T : IInterceptedObject
    {
    }
}
