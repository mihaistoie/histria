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
            IInterceptedObject child = item as IInterceptedObject;
            if (child == null)
            {
                values.RemoveAt(index);
                return;
            }
            if (PropInfo.Role.IsParent)
            {
                if (Instance.AOPBeforeChangeChild(PropInfo.Name, child, RoleOperation.Remove))
                {
                    values.RemoveAt(index);
                    //UpdateForeignKeysAndState(PropInfo.Role.InvRole, child, null, true);
                    Instance.AOPAfterChangeChild(PropInfo.Name, child, RoleOperation.Remove);
                }
            }
            else
            {
                values.RemoveAt(index);
            }

        }
    }
}
