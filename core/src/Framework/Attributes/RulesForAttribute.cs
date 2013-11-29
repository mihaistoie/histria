using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Framework.Attributes
{
    /// <summary>
    /// Allow to define the table name and  primary key for a class
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    class RulesForAttribute : System.Attribute
    {
        public String ClassName; 
        public RulesForAttribute(string className)
        {
            ClassName = className;
        }
    }
}
