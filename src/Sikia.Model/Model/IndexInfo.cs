using System;
using System.Collections.Generic;
using System.Reflection;


namespace Sikia.Model
{
    using Sikia.Sys;

    ///<summary>
    /// Definition of an index: name/unique/fields
    ///</summary>
    public class IndexInfo
    {
        /// A field in an index
        ///</summary>
        ///<summary>
        private class IndexInfoItem
        {
            public string FiledName { get; set; }
            public PropertyInfo Property { get; set; }
            public bool Descendent { get; set; }

   
            public IndexInfoItem(PropertyInfo pi, string fieldName, bool descendent = false)
            {
                FiledName = fieldName;
                Property = pi;
                Descendent = descendent;
            }

        }
       
        private bool Unique { get; set; }
        private string IndexName { get; set; }
        private List<IndexInfoItem> Items = new List<IndexInfoItem>();
        public IndexInfo() { }

        ///<summary>
        /// Load index definition
        ///</summary>
        public void Load(string fields, string indexName, bool unique, ClassInfoItem ci)
        {
            Unique = unique;
            IndexName = indexName;
            string defIndexName = ci.Name;
            string[] aFields = fields.Trim().Split(',');
            foreach (string field in aFields)
            {
                string[] afields = field.Trim().Split(' ');
                bool desc = false;
                if (afields.Length > 1)
                {
                    desc = afields[1].Trim().ToLower() == "desc";
                }
                string sfield = afields[0].Trim();
                PropertyInfo pi = ci.PropertyInfoByName(sfield);
                if (pi == null)
                    throw new ModelException(String.Format(StrUtils.TT("Class {0}: Invalid property {1} for index {2}."), ci.Name, sfield, indexName), ci.Name);
                PropinfoItem pp = ci.Properties[pi];
                defIndexName += '_' + pp.DbName;
                Items.Add(new IndexInfoItem(pi, sfield, desc));
            }
            if (Items.Count == 0)
            {
                throw new ModelException(String.Format(StrUtils.TT("Class {0}: Invalid index definition {1} no fields found."), ci.Name, indexName), ci.Name);
            }
            if (String.IsNullOrEmpty(indexName))
                IndexName = defIndexName;
        }
  
    }
}
