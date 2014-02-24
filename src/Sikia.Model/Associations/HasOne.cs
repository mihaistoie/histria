using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class HasOne<T> : Association, IRoleRef where T : IInterceptedObject
    {
        private IInterceptedObject value;
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
                    UpdateForeignKeysAndState(PropInfo.Role, null, value);
                    Instance.AOPAfterChangeChild(PropInfo.Name, value, RoleOperation.Remove);
                    
                }
                if (!Instance.AOPBeforeChangeChild(PropInfo.Name, iValue, RoleOperation.Add))
                {
                    return;
                }
                UpdateForeignKeysAndState(PropInfo.Role, Instance, iValue);
                value = iValue;
                Instance.AOPAfterChangeChild(PropInfo.Name, value, RoleOperation.Add);

            }
            else
            {
                // Association
                if (!Instance.AOPBeforeChangeChild(PropInfo.Name, iValue, RoleOperation.Update))
                {
                    return;
                }
                UpdateForeignKeysAndState(PropInfo.Role, Instance, iValue);
                value = iValue;
                Instance.AOPAfterChangeChild(PropInfo.Name, value, RoleOperation.Update);
            }
            
           
        }

        public HasOne()
        {
        }
        public IInterceptedObject Value { get { return value; } set { internalSetValue(value); } }
    }
}


