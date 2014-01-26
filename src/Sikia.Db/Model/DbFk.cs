using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbFk
    {
        protected DbFkColumns columns = new DbFkColumns();
        public string FKName { get; set; }
        public string TableName { get; set; }
        public string UniqueTableName { get; set; }
        public DbFkColumns Columns { get { return columns;  } }
    }
}
