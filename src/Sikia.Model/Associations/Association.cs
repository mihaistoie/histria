using Sikia.Sys;
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
        public PropInfoItem PropInfo { get; set; }
        ///<summary>
        /// The instance that contains this association
        ///</summary> 
        public IInterceptedObject Instance { get; set; }
        internal void UpdateForeignKeysAndState(RoleInfoItem role, IInterceptedObject target, IInterceptedObject refObj, bool isComposition)
        {
            if (isComposition && refObj == null)
            {
                target.AOPDeleted();
                //Don't set foreign keys null !!! 
            }
            else
            {
                if (role.FkFieldsExist)
                {
                    //Update FK
                    for (int index = 0; index < role.FkFields.Count; index++)
                    {
                        ForeignKeyInfo fki = role.FkFields[index];
                        if (!fki.ReadOnly)
                        {
                            object value = null;
                            if (refObj != null)
                            {
                                PropInfoItem pai = refObj.ClassInfo.PropertyByName(role.PkFields[index]);
                                value = pai.PropInfo.GetValue(refObj, null);
                            }
                            fki.Prop.PropInfo.SetValue(target, value, null);
                            if (fki.Prop.dependOnMe != null && (fki.Prop.dependOnMe.Count > 1))
                            {
                                foreach (RoleInfoItem r in fki.Prop.dependOnMe)
                                {
                                    if (r == role) continue;
                                    if (r.HasReadOnlyField(fki.Field))
                                    {
                                        object rv = r.RoleProp.PropInfo.GetValue(target, null);
                                        IRoleRef rr = rv as IRoleRef;
                                        if (rr != null)
                                        {
                                            rr.SetValue(null);
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            object value = null;
                            if (refObj != null)
                            {
                                // Read only foreign key must be equal with values of primary key 
                                PropInfoItem pai = refObj.ClassInfo.PropertyByName(role.PkFields[index]);
                                value = pai.PropInfo.GetValue(refObj, null);
                                object existingValue = fki.Prop.PropInfo.GetValue(target, null);
                                if (value != existingValue)
                                {
                                    throw new RuleException(L.T("Invalid value for {0}.{1}, excepted {2}, found {3}."), fki.Prop.ClassInfo.Name, fki.Field, value, existingValue);
                                }
                            }

                        }

                    }
                }
            }
            if (isComposition)
            {
                object roleValue = role.RoleProp.PropInfo.GetValue(target, null);
                IRoleChild child = roleValue as IRoleChild;
                child.SetParent(refObj);
            }


        }


    }
}
