using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core
{
    public class BelongsTo<T> : Association<T>, IRoleRef, IRoleChild where T : InterceptedObject
    {
    }
}
