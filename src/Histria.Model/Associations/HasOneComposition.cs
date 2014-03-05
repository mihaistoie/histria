using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public class HasOneComposition<T> : HasOne<T> where T : IInterceptedObject
    {

        public HasOneComposition()
        {
            _value = default(T);
        }

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

            PropInfoItem inv = PropInfo.Role.InvRole.RoleProp;
            if (ov != null)
            {
                IInterceptedObject old = (ov as IInterceptedObject);
                rc = inv.PropInfo.GetValue(ov, null) as IRoleChild;
                old.AOPDeleted(false);
                if (!rc.SetParent(null, false))
                {
                    return;
                }
            }
            if (!Instance.AOPBeforeSetProperty(PropInfo.Name, ref nv, ref ov))
            {
                return;
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
