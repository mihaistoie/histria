using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbIndex
    {
        protected string indexName;
        private string indexFields;
        protected DbIndexColumns columns = new DbIndexColumns();

        public DbIndexColumns Columns { get { return columns; } }
        public string IndexFields
        {
            get
            {
                if (string.IsNullOrEmpty(indexFields))
                {
                    var inb = new StringBuilder();
                    for (var i = 0; i < columns.Count; ++i)
                    {
                        var cc = columns[i];
                        inb.Append(cc.ColumnName.ToLower()).Append(",");
                    }
                    indexFields = inb.ToString();
                }
                return indexFields;
            }
        }
        public string IndexName
        {
            get
            {
                if (string.IsNullOrEmpty(indexName))
                {
                    var inb = new StringBuilder().Append(TableName);
                    for (var i = 0; i < columns.Count; ++i) 
                    {
                        var cc = columns[i];
                        inb.Append("_").Append(cc.ColumnName);
                        if (cc.Descending)
                            inb.Append("_d");

                    }
                    indexName = inb.ToString();

                }
                return indexName;
            }
            set
            {
                indexName = value;
            }
        }
        public bool Unique { get; set; }
        public string TableName { get; set; }
        public void AddColumn(string columnName, bool descending = false)
        {
            columns.Add(new DbIndexItem() { ColumnName = columnName, Descending = descending });
        }
    }





}


