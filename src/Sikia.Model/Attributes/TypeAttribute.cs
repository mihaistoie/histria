using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class TypeAttribute : System.Attribute
    {
        internal virtual bool Validate(object value, out string errors) 
        {
            errors = null;
            return true;
        }
    }
}
