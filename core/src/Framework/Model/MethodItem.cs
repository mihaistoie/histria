using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace Sikia.Framework.Model
{
    class MethodItem
    {
        private string title;
        private string description;
        public MethodInfo Method;
        public string ClassName { get; set; }
        public string SrcClassName { get; set; }
        public string Title { get { return title; } }
        public string Description { get { return description; } }
        public MethodItem(MethodInfo info)
        {
            Method = info;
            title = Method.Name;
            DisplayAttribute da = Method.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            title = Method.Name;
            if (da != null)
            {
                title = da.Title;
                description = da.Description;
            }
            if (description == "") description = title;
        }

    }
}

