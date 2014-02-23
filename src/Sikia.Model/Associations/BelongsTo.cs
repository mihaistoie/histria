using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class BelongsTo<T> : Association, IRoleRef, IRoleChild where T : IInterceptedObject
    {
        public BelongsTo()
        {
        }
        public virtual IInterceptedObject Value { get { return null; } } 
    }
}
