using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    /// <summary>
    /// This class is ignored  by the model loader
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class NoModelAttribute : System.Attribute
    {
    }
}
