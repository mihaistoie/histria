using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbTable
    {
        protected Dictionary<string, DbColumn> columns = new Dictionary<string, DbColumn>();
        protected List<string> pks = new List<string>();
        protected DbIndexes indexes = new DbIndexes();

        ///<summary>
        /// Primary key columns
        ///</summary>
        public List<string> PKs { get { return pks; } }

        ///<summary>
        /// Table columns
        ///</summary>
        public Dictionary<string, DbColumn> Columns { get { return columns; } }
  
        ///<summary>
        /// Table indexes
        ///</summary>
        public DbIndexes Indexes { get { return indexes; } }
        
        ///<summary>
        /// Table name
        ///</summary
        public string TableName { get; set; }
    }
}
