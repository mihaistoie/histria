using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia
{
    ///<summary>
    /// Instances that support this interface can be add in transaction
    ///</summary>
    public interface ITranObject : IPersistentObj, IInterceptedObject
    {
        Type ClassType();
    }
}


