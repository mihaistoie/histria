using System;
using System.Collections.Generic;
using System.Reflection;


namespace Histria.Model
{
    using Histria.Sys;
    using System.Text;

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
            public string FieldName { get; set; }
            public PropertyInfo Property { get; set; }
            public bool Descendent { get; set; }

   
            public IndexInfoItem(PropertyInfo pi, string fieldName, bool descendent = false)
            {
                this.FieldName = fieldName;
                this.Property = pi;
                this.Descendent = descendent;
            }

        }
       
        internal bool Unique { get; set; }
        internal string IndexName { get; set; }
        private List<IndexInfoItem> Items = new List<IndexInfoItem>();
        internal string ItemsAsString()
        {
            string[] sfields = new string[this.Items.Count];
            for (int i = 0; i < this.Items.Count; i++)
            {
                IndexInfoItem ii = this.Items[i];
                sfields[i] = ii.Descendent ? ii.FieldName  + " desc" : ii.FieldName;
            }
            return string.Join(",", sfields);
        }
 
        public IndexInfo() { }

        ///<summary>
        /// Load index definition
        ///</summary>
        internal void Load(string fields, string indexName, bool unique, ClassInfoItem ci)
        {
            this.Unique = unique;
            this.IndexName = indexName;
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
                    throw new ModelException(String.Format(L.T("Class {0}: Invalid property {1} for index {2}."), ci.Name, sfield, indexName), ci.Name);
                PropInfoItem pp = ci.Properties[pi];
                defIndexName += '_' + pp.DbName;
                this.Items.Add(new IndexInfoItem(pi, sfield, desc));
            }
            if (this.Items.Count == 0)
            {
                throw new ModelException(String.Format(L.T("Class {0}: Invalid index definition {1} no fields found."), ci.Name, indexName), ci.Name);
            }
            if (String.IsNullOrEmpty(indexName))
                this.IndexName = defIndexName;
        }
  
    }
}
