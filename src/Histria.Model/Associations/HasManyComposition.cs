using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    internal class HasManyComposition<T> : HasMany<T>, IRoleParent where T : IInterceptedObject
    {

        #region Interface IRoleParent
        bool IRoleParent.RemoveChildAt(IInterceptedObject child, int index)
        {
            PropInfoItem inv = PropInfo.Role.InvRole.RoleProp;
            IRoleChild rc = inv.PropInfo.GetValue(child, null) as IRoleChild;
            if (!rc.SetParent(null, false))
            {
                return false;
            }
            T val = (T)child;
            if (index < 0) index = values.IndexOf(val);
            InternalRemoveValue(val, index);
            return true;
        }
        bool IRoleParent.RemoveAllChildren()
        {
            bool res = true;
            IRoleParent rp = this as IRoleParent;
            for (int index = values.Count - 1; index >= 0; index--)
            {
                if (!rp.RemoveChildAt(values[index], index))
                    res = false;
            }
            return res;
        }
        #endregion

        #region Add / Remove for compositions

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
            (Instance as IObjectLifetime).Notify(ObjectLifetime.AssociationsChanged, PropInfo.Name, this);
            Instance.AOPAfterChangeChild(PropInfo.Name, child, RoleOperation.Add);
        }

        protected override void Remove(T item, int index)
        {

            IInterceptedObject child = item as IInterceptedObject;

            if (!Instance.AOPBeforeChangeChild(PropInfo.Name, child, RoleOperation.Remove))
            {
                return;
            }
            item.AOPDelete(false);
            (this as IRoleParent).RemoveChildAt(item, index);
            (Instance as IObjectLifetime).Notify(ObjectLifetime.AssociationsChanged, PropInfo.Name, this);
            Instance.AOPAfterChangeChild(PropInfo.Name, child, RoleOperation.Remove);

        }
        #endregion
    }
}
