using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public enum ObjectLifetime
    {
        Created,
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
