using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Framework.Model
{
    public class ModelException : Exception
    {
        public string ClassName;
        public string PropName;
        public ModelException() : base() { }
        public ModelException(string message) : base(message) { }
        public ModelException(string message, string className)
            : base(message)
        {
            ClassName = className;
        }
        public ModelException(string message, string className, string propName)
            : base(message)
        {
            ClassName = className;
            PropName = propName;
        }
    }
}
