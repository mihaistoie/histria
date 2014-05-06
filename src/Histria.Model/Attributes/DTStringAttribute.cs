using Histria.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Histria.Model
{
    /// <summary>
    /// Schema validation for string properties
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class DtStringAttribute : TypeAttribute
    {
        #region Implementation 
        private Regex _regexPattern = null;
        internal override void InitFromTemplate(TypeAttribute template)
        {
            if (template is DtStringAttribute)
            {
                DtStringAttribute tt = (DtStringAttribute)template;
                MaxLength = tt.MaxLength;
                MinLength = tt.MinLength;
                Pattern = tt.Pattern;
                Format = tt.Format;
            }
        }
        #endregion

        #region Properties
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public string Pattern { get; set; }
        public StringFormatType Format { get; set; }
        #endregion

        #region Constructor
        public DtStringAttribute()
        {
            Format = StringFormatType.None;
            MaxLength = 0;
            MinLength = 0;
        }
        #endregion 

        #region Validation
        internal override bool TryValidate(object value, out string errors)
        {
            errors = null;
            string val = (string)value;
            if (string.IsNullOrEmpty(val)) return true;

            if (MaxLength > 0 || MinLength > 0)
            {
                int len = val.Length;
                if (len > MaxLength && MaxLength > 0)
                {
                    errors = L.T("Maximum {0} characters allowed.", MaxLength);
                    return false;
                }
                if (len < MinLength && MinLength > 0)
                {

                    errors = L.T("Minimum {0} characters required.", MinLength);
                    return false;
                }

            }
            if (_regexPattern != null || !string.IsNullOrEmpty(Pattern))
            {
                if (_regexPattern == null)
                {
                    _regexPattern = new Regex(Pattern);
                }
                if (!_regexPattern.IsMatch(val))
                {
                    errors = L.T("Invalid format.");
                    return false;
                }

            }
            return true;
        }
        #endregion
    }

}

