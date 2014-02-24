using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia
{
    public enum RoleOperation
    {
        ///<summary>
        /// A new instance was added to role  
        ///</summary>          
        Add,
        
        ///<summary>
        /// An instance was removed from role  
        ///</summary> 
        Remove,

        ///<summary>
        /// An instance was modified  
        ///</summary> 
        Update
    }
}
