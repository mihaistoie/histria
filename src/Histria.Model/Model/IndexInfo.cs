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
        public class IndexInfoItem
        {
            public string FieldName { get; private set; }
            public PropInfoItem Props { get; private set; }
            public bool Descending { get; private set; }


            public IndexInfoItem(PropInfoItem pi, string fieldName, bool descending = false)
            {
                this.FieldName = fieldName;
                this.Props = pi;
                this.Descending = descending;
            }
        }
        private List<IndexInfoItem> _items = new List<IndexInfoItem>();
        
        public bool Unique { get; private set; }
        internal string IndexName { get; private set; }
        public List<IndexInfoItem> Items { get { return _items; } }

        internal string ItemsAsString()
        {
            string[] sfields = new string[this._items.Count];
            for (int i = 0; i < this._items.Count; i++)
            {
                IndexInfoItem ii = this._items[i];
                sfields[i] = ii.Descending ? ii.FieldName  + " desc" : ii.FieldName;
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
                this._items.Add(new IndexInfoItem(pp, sfield, desc));
            }
            if (this._items.Count == 0)
            {
                throw new ModelException(String.Format(L.T("Class {0}: Invalid index definition {1} no fields found."), ci.Name, indexName), ci.Name);
            }
            if (String.IsNullOrEmpty(indexName))
                this.IndexName = defIndexName;
        }
  
    }
}
