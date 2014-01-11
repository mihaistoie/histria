using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core.Model
{
    
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
