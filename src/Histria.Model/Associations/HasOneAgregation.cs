using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    internal class HasOneAggregation<T> : HasOne<T> where T : IInterceptedObject
    {

        public HasOneAggregation()
        {
            _value = default(T);
        }
    }
}
