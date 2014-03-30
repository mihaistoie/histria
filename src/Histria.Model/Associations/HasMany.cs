using Histria.Sys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Histria.Model
{

    public class HasMany<T> : Association, IEnumerable<T>, IRoleList where T : IInterceptedObject
    {
        #region Internal members
        private List<T> instances = null;
        #endregion

        #region Lazy loading
        protected List<T> values
        {
            get
            {
                if (instances == null)
                {
                    instances = new List<T>();
                }
                return instances;
            }
        }
        #endregion

        #region Properties
        public int Count { get { return values.Count; } }
        
        public T this[int index]
        {
            get
            {
                return values[index];
            }
        }

        #endregion

        #region Notifications

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Add/Remove
        public bool Has(T item)
        {
            return values.IndexOf(item) >= 0;
        }

        protected virtual void Remove(T item, int index)
        {
            values.RemoveAt(index);

        }
        protected void InternalRemoveValue(T item, int index)
        {
            values.RemoveAt(index);
        }

        protected virtual void AddOrInsert(T item, int index)
        {
            InternalAddValue(item, index);
        }

        protected virtual void InternalAddValue(T item, int index)
        {
            if (index >= 0)
                values.Insert(index, item);
            else
                values.Add(item);
        }

        public void Add(T item)
        {
            AddOrInsert(item, -1);
        }

        public void Remove(T item)
        {
            int index = values.IndexOf(item);
            if (index >= 0)
            {
                Remove(item, index);
            }
        }

        public void RemoveAt(int index)
        {
            T item = values[index];
            Remove(item, index);
        }

        #endregion

        #region Interface IRoleList
        void IRoleList.AddOrInsert(IInterceptedObject value, int index)
        {
            if (Has((T)value))
            {
                throw new RuleException(L.T("Item already exists."));
            }
            AddOrInsert((T)value, index);
        }

        void IRoleList.Remove(IInterceptedObject value)
        {
            Remove((T)value);
        }
        #endregion

    }
}
