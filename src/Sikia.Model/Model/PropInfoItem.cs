using Sikia.Sys;
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
        /// Roles which depend on this property
        ///</summary>   
        internal List<RoleInfoItem> dependOnMe = null;

        ///<summary>
        /// Class info 
        ///</summary>   
        internal ClassInfoItem ClassInfo = null;

        ///<summary>
        /// Name of property
        ///</summary>   
        public string Name { get; set; }

        ///<summary>
        /// Column Name - database Mapping
        ///</summary>   
        public string PersistentName { get; set; }

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
        /// Default Value ?
        ///</summary>   
        public object DefaultValue { get; set; }

        ///<summary>
        /// Title of property
        ///</summary>   
        public string Title { get { return titleGet == null ? title : (string)titleGet.Invoke(this, null); } }

        ///<summary>
        /// A short description of property 
        ///</summary>   
        public string Description { get { return descriptionGet == null ? description : (string)descriptionGet.Invoke(this, null); } }

        ///<summary>
        /// Role detail
        ///</summary>   
        internal RoleInfoItem Role { get; set; }

        ///<summary>
        /// IsRole ?
        ///</summary>  
        public bool IsRole { get { return Role != null; } }

        ///<summary>
        /// Schema validation
        ///</summary>   
        internal TypeAttribute TypeValidation;

        ///<summary>
        /// Data type
        ///</summary>   
        internal DataTypes DtType = DataTypes.Unknown;


        #endregion

        #region Loading
        public PropinfoItem(PropertyInfo cPi)
        {
            PropInfo = cPi;
            Name = PropInfo.Name;
            PersistentName = PropInfo.Name;
            title = Name;
            DisplayAttribute da = PropInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            title = Name;
            if (da != null)
            {
                title = da.Title;
                description = da.Description;
            }
            if (string.IsNullOrEmpty(description))
                description = title;
            DefaultAttribute dfa = PropInfo.GetCustomAttributes(typeof(DefaultAttribute), false).FirstOrDefault() as DefaultAttribute;
            if (da != null)
            {
                IsDisabled = dfa.Disabled;
                IsHidden = dfa.Hidden;
                IsMandatory = dfa.Required;
                DefaultValue = dfa.Value;
            }

            if (cPi.PropertyType == typeof(string) || cPi.PropertyType == typeof(String))
            {
                DtType = DataTypes.String;
                TypeValidation = PropInfo.GetCustomAttributes(typeof(DtStringAttribute), false).FirstOrDefault() as DtStringAttribute;
            }

            else if (cPi.PropertyType == typeof(Int64))
            {
                DtType = DataTypes.BigInt;
            }
            else if (cPi.PropertyType == typeof(int))
            {
                DtType = DataTypes.Int;
            }
            else if (cPi.PropertyType == typeof(bool) || cPi.PropertyType == typeof(Boolean))
            {
                DtType = DataTypes.Bool;
            }
            else if (cPi.PropertyType == typeof(short))
            {
                DtType = DataTypes.SmallInt;

            }
            else if (cPi.PropertyType.IsEnum)
            {
                DtType = DataTypes.Enum;

            }
            else if (cPi.PropertyType == typeof(decimal) || cPi.PropertyType == typeof(Decimal)
             || cPi.PropertyType == typeof(double) || cPi.PropertyType == typeof(Double))
            {
                DtType = DataTypes.Number;
            }


        }
                

        internal void AddRole(RoleInfoItem role)
        {
            if (dependOnMe == null)
            {
                dependOnMe = new List<RoleInfoItem>();
            }
            dependOnMe.Add(role);
        }

        internal void AfterLoad(ModelManager model, ClassInfoItem ci)
        {
            if (IsRole)
            {
                // Check role && Load role dependencies

                RoleInfoItem role = Role;
                if (role.IsList && string.IsNullOrEmpty(role.InvRoleName))
                {
                    throw new ModelException(String.Format(StrUtils.TT("Invalid role definition {0}.{1}. Missing 'Inv' attribute."), ci.Name, PropInfo.Name), ci.Name);
                }
                Type invClassType = PropInfo.PropertyType.GetGenericArguments()[0];

                ClassInfoItem remoteClass = model.ClassByType(invClassType);
                if (remoteClass == null)
                {
                    throw new ModelException(String.Format(StrUtils.TT("Invalid role definition {0}.{1}. Remote class not found."), ci.Name, PropInfo.Name), ci.Name);
                }

                if (!string.IsNullOrEmpty(role.InvRoleName))
                {
                    PropinfoItem pp = remoteClass.PropertyByName(role.InvRoleName);
                    if (pp != null)
                    {
                        role.InvRole = pp.Role;
                    }
                    if (role.InvRole == null)
                        throw new ModelException(String.Format(StrUtils.TT("Invalid role definition {0}.{1}. Invalid inv role {2}.{3}."), ci.Name, PropInfo.Name, remoteClass.Name, role.InvRoleName), ci.Name);
                }
                if (role.InvRole != null)
                {
                    //One-to-one relationship 
                    if (role.IsRef && !role.IsChild && !role.InvRole.IsList && role.InvRole.IsChild)
                    {
                        role.IsList = true;
                    }
                }
                if (role.IsRef)
                {
                    role.UsePk = false;
                    if (!string.IsNullOrEmpty(role.ForeignKey))
                    {
                        string[] fks = role.ForeignKey.Split(',');
                        role.PkFields = new string[fks.Length];
                        role.FkFields = new List<ForeignKeyInfo>(fks.Length);
                        role.UsePk = role.ForeignKey.IndexOf("=") < 0;
                        if (role.UsePk && (fks.Length != remoteClass.Key.Count))
                        {
                            throw new ModelException(String.Format(StrUtils.TT("Invalid role definition {0}.{1}. Invalid inv role {2}.{3}."), ci.Name, PropInfo.Name, remoteClass.Name, role.InvRoleName), ci.Name);
                        }
                        int index = 0;
                        foreach (string fk in fks)
                        {
                            if (role.UsePk)
                            {
                                role.FkFields.Add(new ForeignKeyInfo() { Field = fk.Trim() });
                                role.PkFields[index] = remoteClass.Key[index].Key;
                            }
                            else
                            {
                                string ff = fk.Trim();
                                bool readOnly = false;
                                int pos = ff.IndexOf("==");
                                if (pos <= 0)
                                {
                                    pos = ff.IndexOf("=");
                                }
                                else
                                {
                                    readOnly = true;
                                }
                                if (pos <= 0)
                                {
                                    throw new ModelException(String.Format(StrUtils.TT("Invalid role definition {0}.{1}. Invalid foreing key '{2}'."), ci.Name, PropInfo.Name, role.ForeignKey), ci.Name);
                                }
                                role.FkFields.Add(new ForeignKeyInfo() { Field = fk.Substring(0, pos).Trim(), ReadOnly = readOnly });
                                role.PkFields[index] = fk.Substring(pos + (readOnly ? 2 : 1)).Trim();
                            }

                            index++;
                        }

                    }
                    else
                    {
                        //Using Uuid
                        role.UsePk = remoteClass.UseUuidAsPk;
                        role.PkFields = new string[] { ModelConst.UUID };
                        role.FkFields = new List<ForeignKeyInfo>() { new ForeignKeyInfo() { Field = ModelConst.RefProperty(Name) } };
                        role.FkFieldsExist = false;

                    }

                    if (!role.UsePk && role.PkFields.Length == remoteClass.Key.Count)
                    {
                        role.UsePk = true;
                        for (int i = 0; i < role.PkFields.Length; i++)
                        {
                            if (string.Compare(role.PkFields[i], remoteClass.Key[i].Key, true) != 0)
                            {
                                role.UsePk = false;
                            }
                        }
                    }
                    //Check if fields exists  
                    for (int i = 0; i < role.PkFields.Length; i++)
                    {
                        PropinfoItem pp = remoteClass.PropertyByName(role.PkFields[i]);
                        if (pp == null)
                        {
                            throw new ModelException(String.Format(StrUtils.TT("Invalid role definition {0}.{1}. Field not found '{2}.{3}'."), ci.Name, PropInfo.Name, remoteClass.Name, role.PkFields[i]), ci.Name);
                        }

                        if (role.FkFieldsExist)
                        {
                            PropinfoItem fp = ci.PropertyByName(role.FkFields[i].Field);
                            if (fp == null)
                            {
                                throw new ModelException(String.Format(StrUtils.TT("Invalid role definition {0}.{1}. Field not found '{2}.{3}'."), ci.Name, PropInfo.Name, ci.Name, role.FkFields[i]), ci.Name);
                            }
                            role.FkFields[i].Prop = fp;
                            if (fp.PropInfo.PropertyType != pp.PropInfo.PropertyType)
                            {
                                throw new ModelException(String.Format(StrUtils.TT("Invalid role definition {0}.{1}. Type mismatch '{2}({3}.{4}) != {5}({6}.{7})'."),
                                    ci.Name, PropInfo.Name, fp.PropInfo.PropertyType.Name, ci.Name, fp.Name,
                                    pp.PropInfo.PropertyType.Name, remoteClass.Name, pp.Name), ci.Name);
                            }
                            fp.AddRole(role);
                        }

                    }
                }

            }

        }

        #endregion

        #region  Validation
        ///<summary>
        /// Check value (range, length ....) 
        ///</summary>   
        public void SchemaValidation(ref object value)
        {
            if (DtType != null)
            {

            }
        }

        #endregion

        #region Rules
        ///<summary>
        /// Associate a rule at this property
        ///</summary>   
        internal void AddRule(RuleItem ri)
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

