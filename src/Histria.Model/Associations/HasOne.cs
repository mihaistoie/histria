using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public class HasOne<T> : Association, IRoleRef, IEnumerable<T> where T : IInterceptedObject
    {
        #region Internals
        protected T _value;
        protected Guid refUid = Guid.Empty;

        protected  virtual void InternalSetValue(T value, bool updateForeignKeys)
        {
            if (updateForeignKeys)
            {
                AssociationHelper.UpdateForeignKeys(PropInfo, Instance, value);
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
        #endregion

        #region IRoleRef

        void IRoleRef.SetValue(IInterceptedObject value)
        {
            ExternalSetValue((T)value);
        }

        IInterceptedObject IRoleRef.GetValue()
        {
            return _value as IInterceptedObject;
        }
        #endregion


        protected virtual T LazyLoading()
        {
            return _value;
        }


        #region Properties
        public Guid RefUid { get { return refUid; } }
        
        public virtual T Value
        {
            get { return LazyLoading(); }
            set
            {
                ExternalSetValue(value);
            }
        }
        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            yield return LazyLoading();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }
}


