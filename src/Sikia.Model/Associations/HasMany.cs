using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sikia.Model
{


    public class HasMany<T> : Association, IEnumerable<T>, IRoleList where T : IInterceptedObject
    {
        #region Internal members
        private IInterceptedObject parent;
        private List<T> _values = null;
        #endregion

        #region Lazy loading
        private List<T> values
        {
            get
            {
                if (_values == null)
                {
                    _values = new List<T>();
                }
                return _values;
            }
        }
        #endregion

        #region Construction

        private HasMany()
        {

        }

        public HasMany(IInterceptedObject parent)
        {
            this.parent = parent;
        }

        #endregion

        #region Notifications

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Add/Remove
        private void Remove(int index, T item)
        {
            IInterceptedObject child = item as IInterceptedObject;
            if (child == null)
            {
                values.RemoveAt(index);
                return;
            }
            if (PropInfo.Role.IsParent)
            {
                if (Instance.AOPBeforeChangeChild(PropInfo.Name, child, RoleOperation.Remove))
                {
                    values.RemoveAt(index);
                    Instance.AOPAfterChangeChild(PropInfo.Name, child, RoleOperation.Remove);
                }
            }
            else
            {
                values.RemoveAt(index);
            }

        }

        public void Add(T item)
        {
            IInterceptedObject child = item as IInterceptedObject;
            if (child == null)
            {
                values.Add(item);
                return;
            }
            if (PropInfo.Role.IsParent)
            {
                if (Instance.AOPBeforeChangeChild(PropInfo.Name, child, RoleOperation.Add))
                {
                    values.Add(item);
                    //Update for 
                    Instance.AOPAfterChangeChild(PropInfo.Name, child, RoleOperation.Add);
                }
            }
            else
            {
                values.Add(item);
            }

        }

        internal void Remove(T item)
        {
            int index = values.IndexOf(item);
            if (index >= 0)
            {
                Remove(index, item);
            }
        }

        internal void RemoveAt(int index)
        {
            T item = values[index];
            Remove(index, item);
        }

        #endregion

    }
}
