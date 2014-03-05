using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public enum ObjectLifetime
    {
        Created,
        Loaded,
        Changed,
        AssociationsChanged,
        Deleted,
        Disposed
    }

    public interface IObjectLifetime
    {
        void Notify(ObjectLifetime objectLifetime, params object[] arguments);
    }
}
