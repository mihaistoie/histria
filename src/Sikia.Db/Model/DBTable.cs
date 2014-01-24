using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbTable
    {
        protected Dictionary<string, DbColumn> columns = new Dictionary<string, DbColumn>();
        
        public Dictionary<string, DbColumn> Columns { get { return columns; } }
        public string TableName { get; set; }
    }
}
