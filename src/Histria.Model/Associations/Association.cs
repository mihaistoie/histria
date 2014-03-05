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
        //public static Type AssociationType(PropInfoItem propInfo, Type declaredType)
        //{
        //    Type generic = declaredType.GetGenericArguments()[0];
        //    Type hasOne = typeof(HasOne<>);
        //    Type cc = hasOne.MakeGenericType(generic);
        //    if (declaredType == cc)
        //    {
        //        if (propInfo.Role.IsList)
        //        {
        //            return typeof(HasOneComposition<>).MakeGenericType(generic);
        //        }
        //        return declaredType;
        //    }
        //    Type hasMany = typeof(HasMany<>);
        //    cc = hasMany.MakeGenericType(generic);
        //    if (declaredType == cc)
        //    {
        //        if (propInfo.Role.IsParent)
        //        {
        //            return typeof(HasManyComposition<>).MakeGenericType(generic);
        //        }
        //        return declaredType;
        //    }

        //    return declaredType;
        //}

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
        /// Update foreign Keys when a relation changed
        ///</summary> 
        public static void RemoveChildren(IInterceptedObject instance )
        {
        }
        public virtual void ChangeContent() 
        { 
            //Nothing to do, used for AOP interception
        }
    }
}
