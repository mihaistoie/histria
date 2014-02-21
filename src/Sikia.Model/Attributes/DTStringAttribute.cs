using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    /// <summary>
    /// Schema validation for string properties
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class DtStringAttribute : TemplateAttribute
    {
        private string template;
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

    }

}
