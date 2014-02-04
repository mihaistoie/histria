using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core
{
    public enum ObjectState
    {
        Iddle = 0,
        Creating = 1,
        Loading = 2,
        Saving = 4,
        Deleting = 8,
        Disposing = 16,
        Frozen = 32
    }
}
