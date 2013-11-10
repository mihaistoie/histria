using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Framework.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct |
                           System.AttributeTargets.Property |
                           System.AttributeTargets.Enum |
                           System.AttributeTargets.Field |
                           System.AttributeTargets.Method,
                           AllowMultiple = false)
    ]
    class DisplayAttribute : System.Attribute
    {
        public string Title;
        public string Description;
        public DisplayAttribute(string iTitle)
        {
            Title = iTitle;
        }
    }

}
