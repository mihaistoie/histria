using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model.Attributes
{
    /// <summary>
    /// This property is a Time
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class DtTimeAttribute : TypeAttribute
    {
    }
}
