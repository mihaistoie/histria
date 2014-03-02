using Sikia.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class BelongsTo<T> : Association, IRoleRef, IRoleChild where T : IInterceptedObject
    {
        private Guid refUid = Guid.Empty;
        private T _value;
        private bool InternalSetValue(T value, bool updateForeignKeys)
        {
            object nv = value;
            object ov = _value;
            if (ov == nv) return false;
            if (updateForeignKeys)
            {

                if (!Instance.AOPBeforeSetProperty(PropInfo.Name, ref nv, ref ov))
                {
                    return false;
                }
                value = (T)nv;
                UpdateForeignKeys(PropInfo, Instance, value);
                refUid = (value != null) ? value.Uuid : Guid.Empty;
            }
            _value = value;
            if (updateForeignKeys)
            {
                Instance.AOPAfterSetProperty(PropInfo.Name, nv, ov);
            }
            return true;
        }

        private void ExternSetParent(IInterceptedObject value)
        {
            object nv = value;
            object ov = _value;
            if (ov == nv) return;
            object roleInvValue;
            if (ov != null)
            {
                roleInvValue = PropInfo.Role.InvRole.RoleProp.PropInfo.GetValue(ov, null);
                if (roleInvValue is IRoleRef)
                {
                    if (nv != null)
                    {
                        throw new RuleException(L.T("Can't change parent."));
                    }
                    (roleInvValue as IRoleRef).SetValue(null);
                }
            }
            else if (nv != null)
            {
                roleInvValue = PropInfo.Role.InvRole.RoleProp.PropInfo.GetValue(nv, null);
                if (roleInvValue is IRoleRef) 
                {
                    (roleInvValue as IRoleRef).SetValue(Instance);
                }
                else
                {

                }

            }


            //throw new Exception("xxxx");
        }

        bool IRoleChild.SetParent(IInterceptedObject value, bool updateForeignKeys)
        {
            return InternalSetValue((T)value, updateForeignKeys);
        }

        void IRoleRef.SetValue(IInterceptedObject value)
        {
        }

        public Guid RefUid { get { return refUid; } }
        public T Value { get { return _value; } set { ExternSetParent(value); } }

    }
}
