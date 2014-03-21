using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public enum ObjectLifetimeEvent
    {
        Created,
        Loaded,
        Changed,
        StateChanged,
        AssociationsChanged,
        Deleted,
        Disposed
    }

    public interface IObjectLifetime
    {
        void Notify(ObjectLifetimeEvent lifetimeEvent, params object[] arguments);
    }
}
