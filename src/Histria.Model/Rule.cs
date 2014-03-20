using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    ///<summary>
    /// Type of rules: validation, propagation ...  
    ///</summary>   
    public enum Rule
    {
        Unknown,
        ///<summary>
        /// Called after set property and before save
        ///</summary>   
        Validation,
        ///<summary>
        /// Called after set property
        ///</summary>   
        Propagation,
        ///<summary>
        /// Called after an instance is created
        ///</summary>   
        AfterCreate,
        ///<summary>
        /// Called after an  is loaded from db ...
        ///</summary>   
        AfterLoad,
        ///<summary>
        /// Validations before delete
        ///</summary>   
        BeforeDelete,
        ///<summary>
        /// Actions to do after delete
        ///</summary>   
        AfterDelete,
        ///<summary>
        /// Validations before save
        ///</summary>   
        BeforeSave,
        ///<summary>
        /// Corrections before set proerty
        ///</summary>   
        Correction
    };
}
