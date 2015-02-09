using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core
{
    [Flags]
    internal enum ObjectStatus : int
    {
        None = 0,
        Active = 1,
        InCreating = 2,
        Created = 4,
        InLoading = 8,
        Loaded = 16,
        InDeleting = 32,
        Deleted = 64,
        InSaving = 128,
        Saved = 256,
        Frozen = 512,
        NoProppagation = 1024,
        Disposing = 2048,
        
    }
}
