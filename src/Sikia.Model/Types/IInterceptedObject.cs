 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia
{
    using Sikia.Model;
    ///<summary>
    /// Interceptable objects
    ///</summary>
    public interface IInterceptedObject: IModelClass
    {
        #region Properties
        ClassInfoItem ClassInfo { get; set; } 
        #endregion

        #region Interceptors
        ///<summary>
        /// Before set property
        ///</summary>
        bool AOPBeforeSetProperty(string propertyName, ref object value, ref object oldValue);
        ///<summary>
        /// After set property
        ///</summary>
        void AOPAfterSetProperty(string propertyName, object newValue, object oldValue);

        ///<summary>
        /// Before modifying a role (add/remove/update)
        ///</summary>
        bool AOPBeforeChangeChild(string propertyName, IInterceptedObject child, RoleOperation operation);
        ///<summary>
        /// After modifying a role (add/remove/update)
        ///</summary>
        void AOPAfterChangeChild(string propertyName, IInterceptedObject child, RoleOperation operation);

        ///<summary>
        /// After an instance is created
        ///</summary>
        void AOPAfterCreate();

        #endregion
    }
}
