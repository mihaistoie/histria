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
        #region Implementation
        internal override void InitFromTemplate(TypeAttribute template)
        {
            if (template is DtNumberAttribute)
            {
                DtNumberAttribute tt = (DtNumberAttribute)template;
                Decimals = tt.Decimals;
                MinValue = tt.MinValue;
                MaxValue = tt.MaxValue;
            }
        }
        #endregion

        #region Properties

        public int Decimals { get; set; }
        public decimal? MinValue { get; set;  }
        public decimal? MaxValue { get; set; }

        #endregion

        #region Constructor
        public DtNumberAttribute()
        {
            Decimals = -1;
        }
        #endregion

        #region Validation
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
        #endregion
    }
}
