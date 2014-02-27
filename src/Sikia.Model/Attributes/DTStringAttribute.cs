using Sikia.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sikia.Model
{
    /// <summary>
    /// Schema validation for string properties
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class DtStringAttribute : TypeAttribute
    {
        private string template;
        private Regex regexPattern = null;
        private void SetTemplate(string templ)
        {
            template = templ;
        }
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public string Pattern { get; set; }
        public StringFormatType Format { get; set; }
        public string Template { get { return template; } set { SetTemplate(value); } }
        public DtStringAttribute()
        {
            Format = StringFormatType.None;
            MaxLength = 0;
            MinLength = 0;
        }
        internal override bool Validate(object value, out string errors)
        {
            errors = null;
            string val = (string)value;
            if (string.IsNullOrEmpty(val)) return true;

            if (MaxLength > 0 || MinLength > 0)
            {
                int len = string.IsNullOrEmpty(val) ? 0 : val.Length;
                if (len > MaxLength && MaxLength > 0)
                {
                    errors = StrUtils.TT("Maximum {0} characters allowed.", MaxLength);
                    return false;
                }
                if (len < MinLength && MinLength > 0)
                {
                    
                    errors =  StrUtils.TT("Minimum {0} characters required.", MinLength);
                    return false;
                }

            }
            if (regexPattern != null || !string.IsNullOrEmpty(Pattern))
            {
                if (regexPattern == null)
                {
                    regexPattern = new Regex(Pattern);
                }
                if (!regexPattern.IsMatch(val))
                {
                    errors = StrUtils.TT("Invalid format.");
                    return false;
                }

            }
            return true;
        }

    }

}
