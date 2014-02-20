using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model.Attributes
{
    /// <summary>
    /// Schema validation for string properties
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    class StringAttribute : TemplateAttribute
    {
        public string Template { get; set; }
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public string Pattern { get; set; }
        public StringFormatType Format { get; set; }
        public StringAttribute()
        {
            Format = StringFormatType.None;
            MaxLength = 0;
            MinLength = 0;
        }
    
    }

}
