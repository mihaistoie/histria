using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public class PropertiesState
    {
        protected virtual BaseItemState GetItemByName(string key) 
        {
            return null;
        }

        protected virtual BaseItemState GetItemByIndex(int index)
        {
            return null;
        }

        public  BaseItemState this[string key]
        {
            get
            {
                return GetItemByName(key);
            }
        }

        public  BaseItemState this[int index]
        {
            get
            {
                return GetItemByIndex(index);
            }
        }
    }
}
