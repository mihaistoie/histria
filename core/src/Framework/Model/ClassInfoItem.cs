using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections.ObjectModel;
using Sikia.Framework.Attributes;
using Sikia.Framework.Utils;
using Sikia.Framework.Types;

namespace Sikia.Framework.Model
{
    public class PropertiesCollection : KeyedCollection<PropertyInfo, PropinfoItem>
    {
        protected override PropertyInfo GetKeyForItem(PropinfoItem item)
        {
            return item.PropInfo;
        }
    }
    public class KeysCollection : KeyedCollection<PropertyInfo, KeyItem>
    {
        protected override PropertyInfo GetKeyForItem(KeyItem item)
        {
            return item.Property;
        }
    }

    public class RulesCollection : KeyedCollection<MethodInfo, RuleItem>
    {
        protected override MethodInfo GetKeyForItem(RuleItem item)
        {
            return item.Method;
        }
    }

    public enum ClassType { Unknown, Model, ViewModel, Process };

    public class ClassInfoItem
    {
        #region private members
        private readonly Dictionary<string, PropertyInfo> propsMap = new Dictionary<string, PropertyInfo>();
        private readonly PropertiesCollection properties = new PropertiesCollection();
        private List<RuleItem> rulesList = new List<RuleItem>();
        private readonly List<MethodItem> methods = new List<MethodItem>();
        private readonly KeysCollection key = new KeysCollection();
        private readonly List<IndexInfo> indexes = new List<IndexInfo>();
        private bool inherianceResolved = false;
        private string title;
        private string description;
        private MethodInfo titleGet = null;
        private MethodInfo descriptionGet = null;
        #endregion

        public Type ClassTypeInfo { get; set; }
        public ClassType ClassType = ClassType.Unknown;
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Title { get { return titleGet == null ? title : (string)titleGet.Invoke(this, null); } }
        public string Description { get { return descriptionGet == null ? description : (string)descriptionGet.Invoke(this, null); } }
        public string DbName { get; set; }
        public bool Static { get; set; }

        #region Properties/Primary Key/Indexes
        public PropertiesCollection Properties { get { return properties; } }
        public KeysCollection Key { get { return key; } }
        public List<IndexInfo> Indexes { get { return indexes; } }
        #endregion

        private bool _ruleExists(RuleItem ri, List<RuleItem> rl)
        {
            Type dt = ri.Method.GetBaseDefinition().DeclaringType;
            if (ClassTypeInfo.IsSubclassOf(dt))
            {
                foreach (RuleItem rule in rl)
                {
                   if (ri.IsOveriddenOf(rule))
                   {
                       throw new ModelException(String.Format(StrUtils.TT("Rule \"{0}.{1}\": Duplicated rule ({2}.{3}). "), ri.SrcClassName, ri.Name, ri.Kind, ri.Property), Name);
                   }
                }
            }
            return false;
        }
        #region Loading
        // validate class after load
        public void ValidateAndPrepare(Model model)
        {
            foreach (PropinfoItem pi in properties)
            {
                pi.AfterLoad(this);
            }
        }
        public void ResolveInheritance(Model model)
        {
            if (Static) return;
            if (inherianceResolved) return;
            if (ClassTypeInfo.BaseType != null)
            {
                ClassInfoItem pci = model.ClassByType(ClassTypeInfo.BaseType);
                if (pci  != null) 
                {
                    pci.ResolveInheritance(model);
                    // Copy rules from parent
                    List<RuleItem> parentRules = new List<RuleItem>();
                    parentRules.AddRange(pci.rulesList);
                    // add child Rules
                    foreach(RuleItem ri in rulesList) {
                        if (!_ruleExists(ri, parentRules))
                        {
                            parentRules.Add(ri);
                        }
                    }
                    rulesList = parentRules;
                }

            }
            inherianceResolved = true;

        }
        // Prepare memory strutures for faster executing
        public void Loaded(Model model)
        {
            if (Static) return;
            foreach (RuleItem ri in rulesList)
            {

                if (!String.IsNullOrEmpty(ri.Property))
                {
                    //checked peoperty exists  
                    PropinfoItem pi = PropertyByName(ri.Property);
                    if (pi == null)
                        throw new ModelException(String.Format(StrUtils.TT("Rule \"{0}.{1}\": The class \"{2}\" has not the property \"{3}\". "), ri.SrcClassName, ri.Name, ri.ClassName, ri.Property), Name);
                    pi.AddRule(ri);

                }
            }
            rulesList.Clear();
        }

        private void LoadTitle()
        {
            DisplayAttribute da = ClassTypeInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            if (da != null)
            {
                title = da.Title;
                description = da.Description;
            }
            if (description == "") description = title;
            if (Static)
            {
                RulesForAttribute rf = ClassTypeInfo.GetCustomAttributes(typeof(RulesForAttribute), false).FirstOrDefault() as RulesForAttribute;
                if (rf != null)
                {
                    ClassName = rf.ClassName;
                }
            }

        }

        private void LoadProperties()
        {
            if (Static) return;
            PropertyInfo[] props = ClassTypeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in props)
            {
                PropinfoItem item = new PropinfoItem(pi);
                propsMap.Add(item.Name, item.PropInfo);
                properties.Add(item);

            }
        }
        private void LoadMethodsAndRules()
        {
            MethodInfo[] methods = ClassTypeInfo.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (MethodInfo mi in methods)
            {
                if (mi.Name.StartsWith("get_") || mi.Name.StartsWith("set_") || mi.Name.StartsWith("AOP")) continue;

                RuleAttribute[] ras = mi.GetCustomAttributes(typeof(RuleAttribute), false) as RuleAttribute[];
                if ((ras != null) && (ras.Length > 0))
                {
                    foreach (RuleAttribute ra in ras)
                    {
                        if (ra.Rule == RuleType.Unknown)
                        {
                            throw new ModelException(String.Format(StrUtils.TT("Invalid rule type for \"{0}\" in the class \"{1}\"."), mi.Name, Name), Name);
                        }
                        if (!ra.CheckProperty())
                        {
                            throw new ModelException(String.Format(StrUtils.TT("Invalid rule property for \"{0}\" in the class \"{1}\"."), mi.Name, Name), Name);
                        }
                        RuleItem ri = new RuleItem(mi);
                        ri.Kind = ra.Rule;
                        ri.Property = ra.Property;
                        ri.ClassName = ClassName;
                        ri.SrcClassName = Name;
                        rulesList.Add(ri);
                    }
                }
            }

        }
        private void LoadTableNameAndPrimarykey()
        {
            if (Static) return;
            DbAttribute db = ClassTypeInfo.GetCustomAttributes(typeof(DbAttribute), false).FirstOrDefault() as DbAttribute;
            if (db != null)
            {
                ClassType = ClassType.Model;
                DbName = (String.IsNullOrEmpty(db.TableName) ? Name : db.TableName);
                // Load primary key
                string[] akeys = db.Keys.Split(',');
                if (akeys.Length == 0)
                {
                    throw new ModelException(String.Format(StrUtils.TT("Missing Primary key for {0}."), Name), Name);
                }
                foreach (string akey in akeys)
                {
                    string skey = akey.Trim();
                    PropertyInfo pi = PropertyInfoByName(skey);
                    if (pi == null)
                        throw new ModelException(String.Format(StrUtils.TT("Invalid field {0}  in Primary key for {1}."), skey, Name), Name);
                    key.Add(new KeyItem(skey, pi));
                }
            }

        }
        private void LoadIndexes()
        {
            //load indexes 
            object[] attributes = ClassTypeInfo.GetCustomAttributes(typeof(IndexAttribute), false);
            foreach (object arttr in attributes)
            {
                IndexAttribute ia = arttr as IndexAttribute;
                IndexInfo ii = new IndexInfo();
                ii.Load(ia.Columns, ia.Name, ia.Unique, this);
                indexes.Add(ii);
            }
        }


        #endregion
        public PropertyInfo PropertyInfoByName(string propName)
        {
            try
            {
                return propsMap[propName];
            }
            catch (Exception)
            {
                return null;
            }
        }
        public PropinfoItem PropertyByName(string propName)
        {
            try
            {
                return properties[propsMap[propName]];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ClassInfoItem(Type cType, bool staticClass)
        {
            ClassTypeInfo = cType;
            Name = ClassTypeInfo.Name;
            ClassName = Name;
            title = Name;
            Static = staticClass;

            LoadTitle();
            LoadProperties();
            LoadMethodsAndRules();
            LoadTableNameAndPrimarykey();
            LoadIndexes();

        }

    }
}

