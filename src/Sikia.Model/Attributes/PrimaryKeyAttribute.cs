using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    /// <summary>
    /// Allow to declare the primary key for a class
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class PrimaryKeyAttribute : System.Attribute
    {
        public string Keys;
        public PrimaryKeyAttribute(string keys)
        {
            Keys = keys;
        }
    }
}
