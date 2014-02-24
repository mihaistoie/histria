using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    ///<summary>
    /// Base class for all associations  
    ///</summary> 
    public abstract class Association : IAssociation
    {
        ///<summary>
        /// Property info 
        ///</summary> 
        public PropinfoItem PropInfo {get; set;}
        ///<summary>
        /// The instance that contains this association
        ///</summary> 
        public IInterceptedObject Instance { get; set; }
        protected void UpdateForeignKeysAndState(RoleInfoItem role, object parent, object child)
        {
            if (role.IsList)
            {
                RoleInfoItem inv = role.InvRole;
                foreach (string field in role.InvRole.FkFields)
                {
                }
            }
            else
            {

            }
        }
     }
}
