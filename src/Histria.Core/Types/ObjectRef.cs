using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core
{
    internal class ObjectRef<T>
    {
        public T Ref { get; set; }

        private int refCount = 1;
        public int RefCount
        {
            get{return this.refCount;}
            private set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("Count must be positive");
                }
                this.refCount = value;
            }
        }

        public void Add()
        {
            this.refCount++;
        }

        public void Remove()
        {
            this.RefCount = this.refCount - 1;
        }
    }
}
