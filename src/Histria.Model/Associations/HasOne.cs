using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public class HasOne<T> : Association, IRoleRef where T : IInterceptedObject
    {
        protected T _value;
        protected Guid refUid = Guid.Empty;

   
        void IRoleRef.SetValue(IInterceptedObject value)
        {
            ExternalSetValue((T)value);
        }
        
        
        protected virtual T LazyLoading()
        {
            return _value;
        }


        protected virtual void InternalSetValue(T value, bool updateForeignKeys)
        {
            if (updateForeignKeys)
            {
                UpdateForeignKeys(PropInfo, Instance, value);
                refUid = (value != null) ? value.Uuid : Guid.Empty;
            }
            _value = value;
        }

        protected virtual void ExternalSetValue(T value)
        {


            object nv = value;
            object ov = _value;
            if (nv == ov) return;

            if (!Instance.AOPBeforeSetProperty(PropInfo.Name, ref nv, ref ov))
            {
                return;
            }
            //Proceed
            InternalSetValue((T)nv, true);

            Instance.AOPAfterSetProperty(PropInfo.Name, nv, ov);
        }
        public Guid RefUid { get { return refUid; } }
        
        public virtual T Value
        {
            get { return LazyLoading(); }
            set
            {
                ExternalSetValue(value);
            }
        }
    }
}


