using Histria.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    ///<summary>
    /// Base class for all associations  
    ///</summary> 
    public abstract class Association : IAssociation
    {
        public static Association AssociationFactory(PropInfoItem propInfo, Type declaredType)
        {
            Type generic = declaredType.GetGenericArguments()[0];
            Type hasOne = typeof(HasOne<>);
            Type cc = hasOne.MakeGenericType(generic);
            if (declaredType == cc)
            {
                if (propInfo.Role.IsList)
                {
                    return (Association)Activator.CreateInstance(typeof(HasOneComposition<>).MakeGenericType(generic));
                }
                else if (propInfo.Role.Type == Relation.Aggregation)
                {
                    return (Association)Activator.CreateInstance(typeof(HasOneAggregation<>).MakeGenericType(generic));
                }
                return (Association)Activator.CreateInstance(declaredType);
            }

            Type hasMany = typeof(HasMany<>);
            cc = hasMany.MakeGenericType(generic);
            if (declaredType == cc)
            {
                if (propInfo.Role.IsParent)
                {
                    return (Association)Activator.CreateInstance(typeof(HasManyComposition<>).MakeGenericType(generic));
                }
                else if (propInfo.Role.Type == Relation.Aggregation)
                {
                    return (Association)Activator.CreateInstance(typeof(HasManyAggregation<>).MakeGenericType(generic));
                }
            }
            return (Association)Activator.CreateInstance(declaredType);

        }

        ///<summary>
        /// Property info 
        ///</summary> 
        public PropInfoItem PropInfo { get; set; }

        ///<summary>
        /// The instance that contains this association
        ///</summary> 
        public IInterceptedObject Instance { get; set; }

        ///<summary>
        /// Update foreign Keys when a relation changed
        ///</summary> 
        internal void UpdateForeignKeys(PropInfoItem Propinfo, IInterceptedObject target, IInterceptedObject refObj)
        {
            //todo: test if target is in (db) loading  --> if (target.InLoading) return; -
            RoleInfoItem role = Propinfo.Role;
            if (role.FkFieldsExist)
            {
                for (int index = 0, len = role.FkFields.Count; index < len; index++)
                {
                    ForeignKeyInfo fki = role.FkFields[index];
                    object value = (refObj == null) ? null : role.PkFields[index].Prop.PropInfo.GetValue(refObj, null);

                    if (fki.ReadOnly)
                    {
                        if (refObj != null)
                        {
                            // Read only foreign key must be equal with values of primary key 
                            object existingValue = fki.Prop.PropInfo.GetValue(target, null);
                            if (value != existingValue)
                            {
                                throw new RuleException(L.T("Invalid value for {0}.{1}, excepted {2}, found {3}."), fki.Prop.ClassInfo.Name, fki.Field, value, existingValue);
                            }
                        }
                    }
                    else
                    {
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
                }
            }
        }
        ///<summary>
        /// Ckeck Constraints(FK) before delete 
        ///</summary>
        public static void CkeckConstraints(IInterceptedObject instance)
        {
            //TODO check if this instance can be deleted 
        }

        public static bool RemoveMeFromParent(IInterceptedObject instance)
        {

            ClassInfoItem ci = instance.ClassInfo;
            for (int index = 0, len = ci.Roles.Count; index < len; index++)
            {
                PropInfoItem pi = ci.Roles[index];
                if (pi.Role.IsChild && pi.Role.IsRef)
                {
                    Association role = (Association)pi.PropInfo.GetValue(instance, null);
                    IInterceptedObject parent = (role as IRoleRef).GetValue();
                    if (parent != null)
                    {
                        object roleInvValue = pi.Role.InvRole.RoleProp.PropInfo.GetValue(parent, null);
                        if (roleInvValue is IRoleRef)
                        {
                            (roleInvValue as IRoleRef).SetValue(null);
                            return true;
                        }
                        else if (roleInvValue is IRoleList)
                        {
                            (roleInvValue as IRoleList).Remove(instance);
                            return true;
                        }
                    }
                    break;
                }
            }
            return false;
        }

        public static void RemoveChildren(IInterceptedObject instance)
        {
            
            ClassInfoItem ci = instance.ClassInfo;
            for (int index = 0, len = ci.Roles.Count; index < len; index++)
            {
                PropInfoItem pi = ci.Roles[index];
                if (pi.Role.IsParent)
                {
                    Association value = (Association)pi.PropInfo.GetValue(instance, null);
                    IEnumerable<IInterceptedObject> collection = (value as IEnumerable<IInterceptedObject>);
                    foreach (IInterceptedObject ii in collection)
                    {
                        RemoveChildren(ii);
                    }
                    IRoleParent pp = value as IRoleParent;
                    pp.RemoveAllChildren();
  
                }
            }
        }

        ///<summary>
        /// EnumChildren
        ///</summary> 
        public static void EnumChildren(IInterceptedObject instance, bool childrenBefore, Action<IInterceptedObject> callBack)
        {
            ClassInfoItem ci = instance.ClassInfo;
            for (int index = 0, len = ci.Roles.Count; index < len; index++ ) 
            {
                PropInfoItem pi = ci.Roles[index];
                if (pi.Role.IsParent) 
                {
                    Association value = (Association)pi.PropInfo.GetValue(instance, null);
                    IEnumerable<IInterceptedObject> collection = (value as IEnumerable<IInterceptedObject>);
                    foreach(IInterceptedObject ii  in collection) 
                    {
                        if (childrenBefore)
                        {
                            EnumChildren(ii, childrenBefore, callBack);
                            callBack(ii);
                        }
                        else
                        {
                            callBack(ii);
                            EnumChildren(ii, childrenBefore, callBack);
                        }
                      
                    }

                }
            }
        }
    }
}
