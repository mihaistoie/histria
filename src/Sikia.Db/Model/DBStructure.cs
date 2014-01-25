using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbStructure
    {
        protected Dictionary<string, DbTable> tables = new Dictionary<string, DbTable>();
        ///<summary>
        /// List of tables
        ///</summary>
        public Dictionary<string, DbTable> Tables { get { return tables; } }

        #region Database Management : create/drop/exists
        ///<summary>
        /// Create database
        ///</summary>
        public virtual void CreateDatabase(string url)
        {

        }
        ///<summary>
        /// Database esists
        ///</summary>
        public virtual bool DatabaseExists(string url)
        {
            return true;
        }
        ///<summary>
        /// drop database
        ///</summary>
        public virtual void DropDatabase(string url)
        {
        }
        #endregion        

        #region Factory
        public virtual DbTable Table()
        {
            return new DbTable();
        }
        public virtual DbColumn Column()
        {
            return new DbColumn();
        }
        #endregion

        ///<summary>
        /// Load database structure (tables/columns/constraints)
        ///</summary>
        public virtual void Load(string url)
        {
            tables.Clear();
        }
    

    }
}
