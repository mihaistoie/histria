using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Histria.Model
{
    /// <summary>
    /// A public method (visible from interface)
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
    public class MethodAttribute : System.Attribute
    {
        /// <summary>
        /// The method is linked to a property or to a class (Property is empty)
        /// </summary>
        public string Property;
        
    }
}

