using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbFk
    {
        private string indexFields;
        protected DbFkColumns columns = new DbFkColumns();
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
        public string FKName { get; set; }
        public string TableName { get; set; }
        public string UniqueTableName { get; set; }
        public DbFkColumns Columns { get { return columns; } }
    }
}
