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


}

