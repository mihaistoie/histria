using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core
{
    /// <summary>
    /// Allow to define the table name and  primary key for a class
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class RulesForAttribute : System.Attribute
    {
        public Type TargetType;
        public RulesForAttribute(Type targetType)
        {
            TargetType = targetType;
        }
        public RulesForAttribute(string targetType)
        {
            TargetType =  Type.GetType(targetType);
        }
    }
}
