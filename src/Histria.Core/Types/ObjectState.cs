using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core
{
    [Flags]
    public enum ObjectState : int
    {
        None = 0,
        Iddle = 1,
        InCreating = 2,
        Created = 4,
        InLoading = 8,
        Loaded = 16,
        InDeleting = 32,
        Deleted = 64,
        InSaving = 128,
        Saved = 256,
        Frozen = 512,
        Disposing = 2048,
        
    }
}
