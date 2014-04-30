using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    internal class HasManyComposition<T> : HasMany<T>, IRoleParent where T : IInterceptedObject
    {
        protected bool HasInvRole { get { return PropInfo.Role.InvRole != null; } }

        #region Interface IRoleParent
        bool IRoleParent.RemoveChild(IInterceptedObject child)
        {
            if (this.HasInvRole)
            {
                PropInfoItem inv = PropInfo.Role.InvRole.RoleProp;
                IRoleChild rc = inv.PropInfo.GetValue(child, null) as IRoleChild;
                if (!rc.SetParent(null, false))
                {
                    return false;
                }
            }
            T val = (T)child;
            InternalRemoveValue(val);
            return true;
        }
        bool IRoleParent.RemoveAllChildren()
        {
            bool res = true;
            IRoleParent rp = this as IRoleParent;
            for (int index = values.Count - 1; index >= 0; index--)
            {
                if (!rp.RemoveChild(values[index]))
                    res = false;
            }
            return res;
        }
        #endregion

        #region Add / Remove for compositions

        protected override void AddOrInsert(T item, int index = -1)
        {

            IRoleChild rc = null;
            PropInfoItem inv = this.HasInvRole ? PropInfo.Role.InvRole.RoleProp : null;
            IInterceptedObject child = item as IInterceptedObject;
            if (!Instance.AOPBeforeChangeChild(PropInfo.Name, child, RoleOperation.Add))
            {
                return;
            }

            if (inv != null)
            {
                rc = inv.PropInfo.GetValue(item, null) as IRoleChild;
                if (!rc.SetParent(Instance, true))
                {
                    return;
                }
            }
            
            InternalAddValue(item, index);
            (Instance as IObjectLifetime).Notify(ObjectLifetimeEvent.AssociationsChanged, PropInfo.Name, this);
            Instance.AOPAfterChangeChild(PropInfo.Name, child, RoleOperation.Add);
        }

        public override void Remove(T item)
        {

            IInterceptedObject child = item as IInterceptedObject;

            if (!Instance.AOPBeforeChangeChild(PropInfo.Name, child, RoleOperation.Remove))
            {
                return;
            }
            item.AOPDelete(false);
            (this as IRoleParent).RemoveChild(item);
            (Instance as IObjectLifetime).Notify(ObjectLifetimeEvent.AssociationsChanged, PropInfo.Name, this);
            Instance.AOPAfterChangeChild(PropInfo.Name, child, RoleOperation.Remove);
        }
        #endregion
    }
}
