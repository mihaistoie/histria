using System;
using System.Linq;
using System.Reflection;

namespace Sikia.Model
{
    public class RuleItem
    {
        private string title;
        private string description;

        public MethodInfo Method;
        public Type DeclaringType;
        public Type TargetType;
        public Rule Kind { get; set; }
        public string Name { get; set; }
        public string Property { get; set; }
        public string Title { get { return title; } }
        public string Description { get { return description; } }
        public RoleOperation Operation { get; set; }

        public bool Static = false;
        
        public bool IsOveriddenOf(RuleItem ri)
        {
            return (ri.Method.IsVirtual && (Kind == ri.Kind) && (Property == ri.Property) 
                && Method.DeclaringType.IsSubclassOf(ri.Method.DeclaringType)) &&
                (Method.ToString() == ri.Method.ToString());
            
        }
        
        public RuleItem(MethodInfo info)
        {
            Method = info;
            Name = Method.Name;
            Kind = Rule.Unknown;
            title = Method.Name;
            DeclaringType = info.DeclaringType;
            TargetType = null;
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
