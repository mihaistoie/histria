using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sikia.Core
{
 
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct |
                           System.AttributeTargets.Property |
                           System.AttributeTargets.Enum |
                           System.AttributeTargets.Field |
                           System.AttributeTargets.Method,
                           AllowMultiple = false)
    ]
    public class DisplayAttribute : System.Attribute
    {
        public string Title;
        public string Description {get; set;}
        public DisplayAttribute(string iTitle)
        {
            Title = iTitle;
            
        }
    }

}
