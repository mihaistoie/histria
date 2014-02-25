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
        public PropinfoItem PropInfo { get; set; }
        ///<summary>
        /// The instance that contains this association
        ///</summary> 
        public IInterceptedObject Instance { get; set; }
        protected void UpdateForeignKeysAndState(RoleInfoItem role, IInterceptedObject target, IInterceptedObject refObj, bool isComposition)
        {
            if (role.FkFieldsExist)
            {
                if (isComposition && refObj == null)
                {
                    target.AOPDeleted();
                    //Don't set foreign keys null !!! 
                }
                else
                {
                    for (int index = 0; index < role.FkFields.Count; index++)
                    {
                        ForeignKeyInfo fki = role.FkFields[index];
                        if (!fki.ReadOnly)
                        {
                            PropinfoItem pi = target.ClassInfo.PropertyByName(fki.Field);
                            object value = null;
                            if (refObj != null)
                            {
                                PropinfoItem pai = refObj.ClassInfo.PropertyByName(role.PkFields[index]);
                                value = pai.PropInfo.GetValue(refObj, null);
                            }
                            pi.PropInfo.SetValue(target, value, null);
                        }
                    }
                }
                if (isComposition)
                {
                   object roleValue = role.RoleProp.PropInfo.GetValue(target,null);
                   IRoleChild child = roleValue as IRoleChild;
                   child.SetParent(refObj);
                }

            }
        }

    }
}
