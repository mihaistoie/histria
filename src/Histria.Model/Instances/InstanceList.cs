using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model.Instances
{
    class InstanceList<T> : IEnumerable<T> where T : IInterceptedObject
    {

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return null;
           // return values.GetEnumerator();
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
