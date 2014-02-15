 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia
{
    ///<summary>
    /// Interceptable objects
    ///</summary>
    public interface IInterceptedObject
    {
       
        #region Interceptors
        ///<summary>
        /// Before set property
        ///</summary>
        bool AOPBeforeSetProperty(string propertyName, ref object value);
        ///<summary>
        /// After set property
        ///</summary>
        void AOPAfterSetProperty(string propertyName, object value);
        ///<summary>
        /// After an instance is created
        ///</summary>
        void AOPAfterCreate();
        #endregion
    }
}
