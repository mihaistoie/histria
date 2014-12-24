using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace Histria.Model
{
    public class MethodItem
    {
        private string title;
        private string description;
        private bool isInterfaceMethod;
        private string property;
        public MethodInfo Method;
        public Type TargetType { get; set; }
        public Type DeclaringType { get; set; }
        public string Title { get { return title; } }
        public string Description { get { return description; } }
        public string Property { get { return property; } }
        public bool IsInterfaceMethod { get { return isInterfaceMethod; } }
        public MethodItem(MethodInfo info)
        {
            this.Method = info;
            this.title = Method.Name;
            this.DeclaringType = Method.DeclaringType;
            DisplayAttribute da = Method.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            this.title = Method.Name;
            if (da != null)
            {
                this.title = da.Title;
                this.description = da.Description;
            }
            if (string.IsNullOrEmpty(description))
            {
                this.description = this.title;
            }
            MethodAttribute ma = Method.GetCustomAttributes(typeof(MethodAttribute), false).FirstOrDefault() as MethodAttribute;
            if (ma != null)
            {
                isInterfaceMethod = true;
                property = ma.Property;
            }
        }

    }
}

