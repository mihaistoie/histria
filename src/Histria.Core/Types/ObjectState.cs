using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core
{
    public enum ObjectState
    {
        Creating = 0,
        Iddle = 1,
        Loading = 2,
        Saving = 4,
        Deleting = 8,
        Disposing = 16,
        Frozen = 32
    }
}
