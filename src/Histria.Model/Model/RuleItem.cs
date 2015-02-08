using System;
using System.Linq;
using System.Reflection;

namespace Histria.Model
{
    public class RuleItem
    {
        private string _title;
        private string _description;
        private string _property;
        private string _sproperty;

        private void extractProperty(string value)
        {
            this._property = value;
            this._sproperty = null;
            if (!string.IsNullOrEmpty(this._property))
            {
                int i = this._property.IndexOf('.');
                if (i > 0)
                {
                    this._sproperty = this._property.Substring(i + 1);
                    this._property = this._property.Substring(0, i);
                }

            }

        }

        public MethodInfo Method { get; internal set; }
        public Type DeclaringType { get; internal set; }
        public Type TargetType { get; internal set; }
        public Rule Kind { get; internal set; }
        public string Name { get; internal set; }
        public string Property { get { return this._property; } }
        public string SubProperty { get { return this._sproperty; } }
        public string Title { get { return this._title; } }
        public string Description { get { return this._description; } }
        public RoleOperation Operation { get; internal set; }

        public bool Static = false;

        public bool IsOveriddenOf(RuleItem ri)
        {
            return (ri.Method.IsVirtual && (this.Kind == ri.Kind) && (this.Property == ri.Property) && (this.SubProperty == ri.SubProperty)
                && this.Method.DeclaringType.IsSubclassOf(ri.Method.DeclaringType)) &&
                (this.Method.ToString() == ri.Method.ToString());
        }

        public RuleItem(MethodInfo info, string property)
        {
            this.Method = info;
            this.Name = Method.Name;
            this.Kind = Rule.Unknown;
            this._title = Method.Name;
            this.DeclaringType = info.DeclaringType;
            this.TargetType = null;
            DisplayAttribute da = this.Method.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            this._title = Method.Name;
            if (da != null)
            {
                this._title = da.Title;
                this._description = da.Description;
            }
            if (string.IsNullOrEmpty(this._description)) 
                this._description = _title;
            this.extractProperty(property);
        }

    }
}
