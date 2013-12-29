using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Framework.Model
{
    ///<summary>
    /// Type of rules: validation, propagation ...  
    ///</summary>   
    public enum RuleType
    {
        Unknown = 0, Validation = 2, Propagation = 4, AfterCreate = 8,
        AfterLoaded = 16, BeforeSave = 32, Correction = 64
    };

    ///<summary>
    /// Type of associations : association, composition, embedded  
    ///</summary>   
    public enum Relation
    {
        Association, Composition, Embedded
    };


    ///<summary>
    /// Type classes 
    ///</summary>   
    public enum ClassType { Unknown, Model, ViewModel, Process };

    ///<summary>
    /// Helper for attribute parsing
    ///</summary>   
    public static class AttributeParser
    {
        ///<summary>
        /// Convert string to RuleType
        ///</summary>   
        static public RuleType ParseRuleType(string value)
        {
           foreach (RuleType enumValue in Enum.GetValues(typeof(RuleType)))
            {
                string svalue = Enum.GetName(typeof(RuleType), enumValue);
                if (String.Compare(svalue, value, true) == 0) return enumValue;
            }
            if (String.Compare("create", value, true) == 0) return RuleType.AfterCreate;
            if (String.Compare("loaded", value, true) == 0) return RuleType.AfterLoaded;
            if (String.Compare("save", value, true) == 0) return RuleType.BeforeSave;
            if (String.Compare("validate", value, true) == 0) return RuleType.Validation;
            if (String.Compare("corection", value, true) == 0) return RuleType.Correction;
            return RuleType.Unknown;
        }
       
    }

    ///<summary>
    /// Helper for model
    ///</summary>   
    public static class ModelHelper
    {
        ///<summary>
        /// get title/description 
        ///</summary>   
        public static string GetStringValue(string value, MethodItem mi)
        {
            if (mi == null)
                return value;
            return (string)mi.Method.Invoke(null, null);
        }

    }


}
