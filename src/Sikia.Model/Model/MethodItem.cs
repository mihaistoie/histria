using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace Sikia.Model
{
    public class MethodItem
    {
        private string title;
        private string description;
        public MethodInfo Method;
        public Type TargetType { get; set; }
        public Type DeclaringType { get; set; }
        public string Title { get { return title; } }
        public string Description { get { return description; } }
        public MethodItem(MethodInfo info)
        {
            Method = info;
            title = Method.Name;
            DeclaringType = Method.DeclaringType;
            DisplayAttribute da = Method.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            title = Method.Name;
            if (da != null)
            {
                title = da.Title;
                description = da.Description;
            }
            if (string.IsNullOrEmpty(description))
            {
                description = title;
            }
        }

    }
}

