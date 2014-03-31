using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public class TypeAttribute : System.Attribute
    {
        internal virtual bool TryValidate(object value, out string errors) 
        {
            errors = null;
            return true;
        }
        internal virtual object SchemaValidation(object value) 
        {
            return value;
        }
    }
}
