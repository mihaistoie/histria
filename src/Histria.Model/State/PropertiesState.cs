using System.Collections;
using System.Collections.Generic;

namespace Histria.Model
{
    public abstract class PropertiesState : IEnumerable<PropertyState>
    {
        protected abstract PropertyState GetItemByName(string key);
        public  PropertyState this[string key]
        {
            get
            {
                return GetItemByName(key);
            }
        }
        public abstract IEnumerator<PropertyState> GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public abstract void Init(ClassInfoItem ci);

    }
}
