using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sikia.Framework.Utils;
using Sikia.Framework.Types;

namespace Sikia.Framework.Model
{
    public class PropinfoItem
    {
        private string title;
        private string description;
        private MethodInfo titleGet = null;
        private MethodInfo descriptionGet = null;
        private readonly Dictionary<RuleType, RuleList> rules = new Dictionary<RuleType, RuleList>();

        public PropertyInfo PropInfo;
        public string Name { get; set; }
        public string DbName { get; set; }
        public string Title { get { return titleGet == null ? title : (string)titleGet.Invoke(this, null); } }
        public string Description { get { return descriptionGet == null ? description : (string)descriptionGet.Invoke(this, null); } }
        public PropinfoItem(PropertyInfo cPi)
        {
            PropInfo = cPi;
            Name = PropInfo.Name;
            DbName = PropInfo.Name;
            title = Name;
            DisplayAttribute da = PropInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            title = Name;
            if (da != null)
            {
                title = da.Title;
                description = da.Description;
            }
            if (description == "") description = title;

        }
        public void AfterLoad(ClassInfoItem ci)
        {
           
        }
        public void AddRule(RuleItem ri)
        {
            RuleList rl = null;
            if (!rules.ContainsKey(ri.Kind))
            {
                rl = new RuleList();
                rules[ri.Kind] = rl;
            }
            else
            {
                rl = rules[ri.Kind];
            }
            rl.Add(ri);
        }
        public void ExecuteRules(RuleType kind, Object target)
        {
            if (rules.ContainsKey(kind))
            {
                rules[kind].Execute(target);
            }
            
        }
    }
}

