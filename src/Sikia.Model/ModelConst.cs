using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public static class ModelConst
    {
        public static string UUID = "Uuid";
        public static string RefProperty(string propName) { return "uid" + propName; }
    }
}
