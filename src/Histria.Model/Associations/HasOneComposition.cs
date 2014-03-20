using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    internal class HasOneComposition<T> : HasOne<T>, IRoleParent where T : IInterceptedObject
    {

        public HasOneComposition()
        {
            _value = default(T);
        }


        #region Interface IRoleParent
        bool IRoleParent.RemoveChildAt(IInterceptedObject child, int index)
        {
            if (child == null)
            {
                child = _value;
                if (child == null) return true;
            }

            PropInfoItem inv = PropInfo.Role.InvRole.RoleProp;
            IRoleChild rc = inv.PropInfo.GetValue(child, null) as IRoleChild;
            if (!rc.SetParent(null, false))
            {
                return false;
            }
            InternalSetValue(default(T), true);
            return true;
        }
       
        bool IRoleParent.RemoveAllChildren()
        {
            return (this as IRoleParent).RemoveChildAt(null, -1);

        }
        #endregion


        protected override void InternalSetValue(T value, bool updateForeignKeys)
        {
            if (updateForeignKeys)
            {
                refUid = (value != null) ? value.Uuid : Guid.Empty;
            }
            _value = value;
        }

        protected override void ExternalSetValue(T value)
        {
            IRoleChild rc = null;
            object nv = value;
            object ov = _value;
            if (nv == ov) return;

            if (!Instance.AOPBeforeSetProperty(PropInfo.Name, ref nv, ref ov))
            {
                return;
            }
            PropInfoItem inv = PropInfo.Role.InvRole.RoleProp;
            if (ov != null)
            {
                IInterceptedObject old = (ov as IInterceptedObject);
                rc = inv.PropInfo.GetValue(ov, null) as IRoleChild;
                old.AOPDelete(false);
                if (!rc.SetParent(null, false))
                {
                    return;
                }
            }
            //Proceed
            if (nv != null)
            {
                rc = inv.PropInfo.GetValue(nv, null) as IRoleChild;
                if (!rc.SetParent(Instance, true))
                {
                    return;
                }
            }
            InternalSetValue((T)nv, true);
            Instance.AOPAfterSetProperty(PropInfo.Name, nv, ov);
        }

    }
}
