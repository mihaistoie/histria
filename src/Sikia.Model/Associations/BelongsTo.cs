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
        public IInterceptedObject Value { get;  internal set; } 
    }
}
