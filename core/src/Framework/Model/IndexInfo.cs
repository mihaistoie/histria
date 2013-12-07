using System;
using System.Collections.Generic;
using System.Reflection;


namespace Sikia.Framework.Model
{
    using Sikia.Framework.Utils;
    public class IndexInfo
    {
        public bool Unique { get; set; }
        public string IndexName { get; set; }
        public List<IndexInfoItem> Items = new List<IndexInfoItem>();
        public IndexInfo() { }

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
                defIndexName += '_' +  pp.DbName;
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
