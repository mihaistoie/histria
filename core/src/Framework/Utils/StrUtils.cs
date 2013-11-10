using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Framework.Utils
{
    class StrUtils
    {
        public static string Namespace2Name(string value)
        {
            var i = value.LastIndexOf('.');
            if (i > 0) return value.Substring(i + 1);
            return value;
        }
    }
}
