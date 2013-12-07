using System.Linq;
using System.Reflection;

namespace Sikia.Framework.Model
{
    public class RuleItem
    {
        private string title;
        private string description;

        public MethodInfo Method;
        public string SrcClassName { get; set; }
        public string ClassName { get; set; }
        public RuleType Kind { get; set; }
        public string Name { get; set; }
        public string Property { get; set; }
        public string Title { get { return title; } }
        public string Description { get { return description; } }
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
            Kind = RuleType.Unknown;
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
