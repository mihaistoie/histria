using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class HasManyComposition<T>: HasMany<T> where T : IInterceptedObject
    {
        protected override void AddOrInsert(T item, int index = -1)
        {
            IRoleChild rc = null;
            PropInfoItem inv = PropInfo.Role.InvRole.RoleProp;
            IInterceptedObject child = item as IInterceptedObject;
           if (!Instance.AOPBeforeChangeChild(PropInfo.Name, child, RoleOperation.Add))
           {
                return;
           }
           rc = inv.PropInfo.GetValue(item, null) as IRoleChild;
           if (!rc.SetParent(Instance, true))
           {
               return;
           }
           InternalAddValue(item, index);
           Instance.AOPBeforeChangeChild(PropInfo.Name, child, RoleOperation.Add);
        }

        protected override void Remove(T item, int index)
        {

            IRoleChild rc = null;
            PropInfoItem inv = PropInfo.Role.InvRole.RoleProp;
            IInterceptedObject child = item as IInterceptedObject;
            
            if (!Instance.AOPBeforeChangeChild(PropInfo.Name, child, RoleOperation.Remove))
            {
                return;
            }
            rc = inv.PropInfo.GetValue(item, null) as IRoleChild;
            if (!rc.SetParent(null, false))
            {
                return;
            }
            InternalRemoveValue(item, index);

            Instance.AOPBeforeChangeChild(PropInfo.Name, child, RoleOperation.Remove);

        }
    }
}
