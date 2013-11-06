using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Framework.Attributes
{
    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Enum,
                           AllowMultiple = false)
    ]
    class EnumCaptions : System.Attribute
    {
        public List<string> Titres;
        public EnumCaptions(params string[] args)
        {
            Titres = new List<string>(args);
        }
    }


    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct |
                           System.AttributeTargets.Property |
                           System.AttributeTargets.Enum |
                           System.AttributeTargets.Field |
                           System.AttributeTargets.Method,
                           AllowMultiple = false)
    ]
    class Display : System.Attribute
    {
        public string Title;
        public string Description;
        public Display(string iTitle)
        {
            Title = iTitle;
        }
    }
}

