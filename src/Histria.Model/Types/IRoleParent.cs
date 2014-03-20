using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria
{
    public interface IRoleParent
    {
        ///<summary>
        /// Remove a  child without notifications (no notifications and no rules)
        ///</summary>
        bool RemoveChildAt(IInterceptedObject child, int index);
       
        ///<summary>
        /// Remove all children without notifications (no notifications and no rules)
        ///</summary>
        bool RemoveAllChildren();
    }
}
