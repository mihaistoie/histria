using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core
{
    public class BelongsTo<T> : Association<T> where T : InterceptedObject, IRoleRef, IRoleChild
    {
    }
}
