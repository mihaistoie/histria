using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core
{
    public class TranObject: InterceptedObject, ITranObject
    {
        #region Interfaces
        ///<summary>
        /// ITranObject.ClassType
        ///</summary>
        public Type ClassType()
        {
            return ClassInfo.CurrentType;
        }
        #endregion
 
    }
}
