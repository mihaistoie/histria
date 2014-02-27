using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class HasOne<T> : Association, IRoleRef where T : IInterceptedObject
    {
        private IInterceptedObject value;
        void IRoleRef.SetValue(IInterceptedObject iValue)
        {
            internalSetValue(iValue);
        }

        private void internalSetValue(IInterceptedObject iValue)
        {
            if (iValue == value) return;
            IInterceptedObject oldValue = value;  
            if (PropInfo.Role.IsList)
            {
                // Composition 
                if (value != null)
                {
                    if (!Instance.AOPBeforeChangeChild(PropInfo.Name, value, RoleOperation.Remove))
                    {
                        return;
                    }
                    UpdateForeignKeysAndState(PropInfo.Role.InvRole, value, null, true);
                    Instance.AOPAfterChangeChild(PropInfo.Name, value, RoleOperation.Remove);
                    
                }
                if (!Instance.AOPBeforeChangeChild(PropInfo.Name, iValue, RoleOperation.Add))
                {
                    return;
                }
                value = iValue;
                UpdateForeignKeysAndState(PropInfo.Role.InvRole, iValue, Instance, true);
                Instance.AOPAfterChangeChild(PropInfo.Name, value, RoleOperation.Add);
            }
            else
            {
                // Association
                if (!Instance.AOPBeforeChangeChild(PropInfo.Name, iValue, RoleOperation.Update))
                {
                    return;
                }
                value = iValue;
                UpdateForeignKeysAndState(PropInfo.Role, Instance, iValue, false);
                Instance.AOPAfterChangeChild(PropInfo.Name, value, RoleOperation.Update);
            }
            
           
        }

        public HasOne()
        {
        }
        public IInterceptedObject Value { get { return value; } set { internalSetValue(value); } }
    }
}


