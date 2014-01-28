using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbColumn
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public DbType Type { get; set; }
        public string  DbType { get; set; }
        public int Size { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public  bool Nullable { get; set; }
        public DbColumn()
        {
            Size = 0;
            Precision = 0;
            Scale = 0;
            Nullable = true;
        }
        public virtual void CreateSQL(StringBuilder sql) {}
    }
}
