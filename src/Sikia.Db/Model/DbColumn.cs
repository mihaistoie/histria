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
        public virtual DbType Type { get; set; }
        public  bool Nullable { get; set; }
    }
}
