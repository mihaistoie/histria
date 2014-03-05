using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model.Attributes
{
    /// <summary>
    /// Schema validation for numbers
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    class DtNumber : TypeAttribute
    {
        public int? Decimals { get; set; }
        public decimal? MinValue { get; set;  }
        public decimal? MaxValue { get; set; }
        public string Template { get; set; }
        internal override bool TryValidate(object value, out string errors)
        {
            errors = null;
            decimal dv = (decimal)value;
            if (MinValue != null)
            {

            }
            if (MaxValue != null)
            {

            }
            if (Decimals != null)
            {

            }
            return true;
        }
    }
}
