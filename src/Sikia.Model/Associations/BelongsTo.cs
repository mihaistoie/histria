using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class BelongsTo<T> : Association<T>, IRoleRef, IRoleChild where T : IInterceptedObject
    {
    }
}
