using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Histria.Core
{
    internal class ObjectRefCollection<T>: KeyedCollection<T, ObjectRef<T>>
    {
        protected override T GetKeyForItem(ObjectRef<T> item)
        {
            return item.Ref;
        }

        public bool TryGetValue(T key, out ObjectRef<T> value)
        {
            if (this.Dictionary == null)
            {
                value = null;
                return false;
            }
            return this.Dictionary.TryGetValue(key, out value);
        }

        public int AddRef(T instance)
        {
            ObjectRef<T> reference;
            if (!this.TryGetValue(instance, out reference))
            {
                reference = new ObjectRef<T>() { Ref = instance };
                Add(reference);
            }
            else
            {
                reference.Add();
            }
            return reference.RefCount;
        }

        public int RemoveRef(T instance)
        {
            ObjectRef<T> reference;
            if (!this.TryGetValue(instance, out reference))
            {
                throw new InvalidOperationException("Instance not found");
            }
            reference.Remove();
            int refCount = reference.RefCount;
            if ( refCount <= 0)
            {
                this.Remove(reference);
            }
            return refCount;
        }

        public ICollection<T> Instances
        {
            get 
            {
                return this.Dictionary == null ? new List<T>() : this.Dictionary.Keys; 
            }
        }
    }
}
