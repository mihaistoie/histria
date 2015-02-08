 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria
{
    using Histria.Model;
    ///<summary>
    /// Interceptable objects
    ///</summary>
    public interface IInterceptedObject: IClassModel
    {
        #region Properties
        ClassInfoItem ClassInfo { get; set; }
        string ObjectPath();
        #endregion

        #region Interceptors


        #region Property Change
        ///<summary>
        /// Before set property
        ///</summary>
        bool AOPBeforeSetProperty(string propertyName, ref object value, ref object oldValue);
        ///<summary>
        /// After set property
        ///</summary>
        void AOPAfterSetProperty(string propertyName, object newValue, object oldValue);

        ///<summary>
        /// Change property
        ///</summary>
        void AOPChangeProperty(PropInfoItem pi, string subProperty, Action changeAction);

        
        
        #endregion

        #region Role Change
        ///<summary>
        /// Before modifying a role (add/remove/update)
        ///</summary>
        bool AOPBeforeChangeChild(string propertyName, IInterceptedObject child, RoleOperation operation);
        ///<summary>
        /// After modifying a role (add/remove/update)
        ///</summary>
        void AOPAfterChangeChild(string propertyName, IInterceptedObject child, RoleOperation operation);
        #endregion

        #region Create / Load
        ///<summary>
        /// After an instance is created
        ///</summary>
        void AOPCreate();

        ///<summary>
        /// Start loading instance
        ///</summary>
        void AOPBeginLoad();

        ///<summary>
        /// Start loading instance
        ///</summary>

        void AOPEndLoad();
        ///<summary>
        /// End loading instance
        ///</summary>

        #endregion

        #region Delete
        ///<summary>
        /// Try to delete an instance
        ///</summary>
        void AOPDelete(bool notifyParent = true);

        #endregion

        #endregion
    }
}
