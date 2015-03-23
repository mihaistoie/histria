using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Db.Model
{
    public class DbFk
    {
        private string _indexFields;
        protected DbFkColumns columns = new DbFkColumns();
        public void AddColumn(string columnName, string uniqueColumnName)
        {
            DbFkItem fk = new DbFkItem();
            fk.ColumnName = columnName;
            fk.UniqueColumnName = uniqueColumnName;
            columns.Add(fk);
        }

        public string IndexFields
        {
            get
            {
                if (string.IsNullOrEmpty(_indexFields))
                {
                    var inb = new StringBuilder();
                    for (var i = 0; i < columns.Count; ++i)
                    {
                        var cc = columns[i];
                        inb.Append(cc.ColumnName.ToLower()).Append(",");
                    }
                    _indexFields = inb.ToString();
                }
                return _indexFields;
            }
        }
        public string FKName { get; set; }
        public bool OnDeleteCascade { get; set; }
        public bool OnDeleteSetNull { get; set; }
        public string TableName { get; set; }
        public string UniqueTableName { get; set; }
        public DbFkColumns Columns { get { return columns; } }
    }
}
