using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sikia.Model
{

    ///<summary>
    /// Provides access to  property metadata
    ///</summary>   
    public class PropinfoItem
    {
        #region Internal fields
        // Static title and description
        private string title;
        private string description;
        // Handlers to get title and description
        private MethodInfo titleGet = null;
        private MethodInfo descriptionGet = null;
        // Rules by type
        private readonly Dictionary<Rule, RuleList> rules = new Dictionary<Rule, RuleList>();
        #endregion

        #region Properties
        public PropertyInfo PropInfo;
        
        ///<summary>
        /// Name of property
        ///</summary>   
        public string Name { get; set; }
        
        ///<summary>
        /// Column Name - database Mapping
        ///</summary>   
        public string DbName { get; set; }

        ///<summary>
        /// Is stored in db ?
        ///</summary>   
        public bool IsPersistent { get; set; }

        ///<summary>
        /// Default is required ?
        ///</summary>   
        public bool IsMandatory { get; set; }

        ///<summary>
        /// Default is hidden ?
        ///</summary>   
        public bool IsHidden { get; set; }

        ///<summary>
        /// Default is disabled ?
        ///</summary>   
        public bool IsDisabled { get; set; }

        
        ///<summary>
        /// Title of property
        ///</summary>   
        public string Title { get { return titleGet == null ? title : (string)titleGet.Invoke(this, null); } }
        
        ///<summary>
        /// A short description of property 
        ///</summary>   
        public string Description { get { return descriptionGet == null ? description : (string)descriptionGet.Invoke(this, null); } }
        #endregion

        #region Loading
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
        #endregion

        #region Rules
        ///<summary>
        /// Associate a rule at this property
        ///</summary>   
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
        ///<summary>
        /// Check value (range, length ....) 
        ///</summary>   
        public void SchemaValidation(ref Object value)
        {
        }
        ///<summary>
        /// Execute rules by type
        ///</summary>   
        public void ExecuteRules(Rule kind, Object target)
        {
            if (rules.ContainsKey(kind))
            {
                rules[kind].Execute(target);
            }

        }
        #endregion
    }
}

