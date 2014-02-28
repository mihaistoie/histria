using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class HasOne<T> : Association, IRoleRef where T : IInterceptedObject
    {
        private IInterceptedObject _value;
        private Guid refUid = Guid.Empty;
        void IRoleRef.SetValue(IInterceptedObject iValue)
        {
            internalSetValue(iValue);
        }

        private void internalSetValue(IInterceptedObject iValue)
        {
            if (iValue == _value) return;
            IInterceptedObject oldValue = _value;
            if (PropInfo.Role.IsList)
            {
                if (_value != null)
                {
                    if (!Instance.AOPBeforeChangeChild(PropInfo.Name, _value, RoleOperation.Remove))
                    {
                        return;
                    }
                    UpdateForeignKeysAndState(PropInfo.Role.InvRole, _value, null, true);
                    Instance.AOPAfterChangeChild(PropInfo.Name, _value, RoleOperation.Remove);

                }
                if (!Instance.AOPBeforeChangeChild(PropInfo.Name, iValue, RoleOperation.Add))
                {
                    return;
                }
                _value = iValue;
                UpdateForeignKeysAndState(PropInfo.Role.InvRole, iValue, Instance, true);
                Instance.AOPAfterChangeChild(PropInfo.Name, _value, RoleOperation.Add);
            }
            else
            {
                // Association
                if (!Instance.AOPBeforeChangeChild(PropInfo.Name, iValue, RoleOperation.Update))
                {
                    return;
                }
                _value = iValue;
                UpdateForeignKeysAndState(PropInfo.Role, Instance, iValue, false);
                refUid = (_value != null) ? _value.Uuid : Guid.Empty;
                Instance.AOPAfterChangeChild(PropInfo.Name, _value, RoleOperation.Update);
            }


        }

        public HasOne()
        {
        }
        public Guid RefUid { get { return refUid; } }
        public IInterceptedObject Value { get { return _value; } set { internalSetValue(value); } }
    }
}


