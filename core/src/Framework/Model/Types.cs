using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Framework.Model
{
    ///<summary>
    /// Type of rules: validation, propagation ...  
    ///</summary>   
    public enum Rule
    {
        Unknown = 0, Validation = 2, Propagation = 4, AfterCreate = 8,
        AfterLoaded = 16, BeforeSave = 32, Correction = 64,
        Required = 128
    };

    ///<summary>
    /// Type of associations : association, composition, embedded  
    ///</summary>   
    public enum Relation
    {
        Association, Composition, Embedded
    };


    ///<summary>
    /// Type classes 
    ///</summary>   
    public enum ClassType { Unknown, Model, ViewModel, Process };

    
    ///<summary>
    /// Helper for model
    ///</summary>   
    public static class ModelHelper
    {
        ///<summary>
        /// get title/description 
        ///</summary>   
        public static string GetStringValue(string value, MethodItem mi)
        {
            if (mi == null)
                return value;
            return (string)mi.Method.Invoke(null, null);
        }

    }


}
