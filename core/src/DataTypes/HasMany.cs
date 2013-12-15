﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sikia.Framework;

namespace Sikia.DataTypes
{


    public class HasMany<T> : IEnumerable<T>
    {
        #region Internal members
        private InterceptedObject parent;
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

        public HasMany(InterceptedObject parent)
        {
            this.parent = parent;
        }

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

        public void Add(T item)
        {
            // notify parent  before add
            values.Add(item);
            // notify parent  after add
        }

        public void AddRange(IEnumerable<T> collection)
        {
            // notify parent  before add
            values.AddRange(collection);
            // notify parent  after add
        }
        
        public void Remove(T item)
        {
            // notify parent  before remove
            values.Remove(item);
            // notify parent  after remove
        }

        public void RemoveAt(int index)
        {
            // notify parent  before remove
            values.RemoveAt(index);
            // notify parent  after remove
        }

        #endregion

    }
}
