using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    /// <summary>
    /// Schema validation for numbers
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class DtNumberAttribute : TypeAttribute
    {
        public int Decimals { get; set; }
        public decimal? MinValue { get; set;  }
        public decimal? MaxValue { get; set; }
        public string Template { get; set; }

        public DtNumberAttribute()
        {
            Decimals = -1;
        }

        internal override object SchemaValidation(object value)
        {
            decimal dv = (decimal)value;
            if (Decimals >= 0) 
            {
                dv = Math.Round(dv, Decimals, MidpointRounding.AwayFromZero);
            }
            return dv;
        }
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
            if (Decimals >= 0)
            {

            }
            return true;
        }
    }
}
