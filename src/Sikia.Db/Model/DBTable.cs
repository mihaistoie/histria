using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbTable
    {
        protected DbColumns columns = new DbColumns();
        protected List<string> pk = new List<string>();
        protected DbIndexes indexes = new DbIndexes();
        protected List<DbFk> foreignKeys = new List<DbFk>();
        
        ///<summary>
        /// Primary key columns
        ///</summary>
        public List<string> PK { get { return pk; } }

        ///<summary>
        /// Table columns
        ///</summary>
        public DbColumns Columns { get { return columns; } }
  
        ///<summary>
        /// Table indexes
        ///</summary>
        public DbIndexes Indexes { get { return indexes; } }
       
        ///<summary>
        /// Foreign keys
        ///</summary>
        public List<DbFk> ForeignKeys { get { return foreignKeys; } }

        ///<summary>
        /// Table name
        ///</summary
        public string TableName { get; set; }
    }
}
