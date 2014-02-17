using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbSchema
    {
        protected DbTables tables = new DbTables();
        /////<summary>
        ///// List of tables
        /////</summary>
        public DbTables Tables { get { return tables; } }

        ///<summary>
        /// Get Table By Name (case insensitive)
        ///</summary>
        public DbTable TableByName(string tableName) 
        {
            try
            {
                return tables[tableName.ToLower()];
            }
            catch
            {
                return null;
            }
        }
        ///<summary>
        /// Returns true if database contains tableName
        ///</summary>
        public bool ContainsTable(string tableName)
        {
            return tables.Contains(tableName.ToLower());
        }


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

        #region Structure
        ///<summary>
        /// Load database structure (tables/columns/constraints)
        ///</summary>
        public virtual void Load(string url)
        {
            tables.Clear();
        }

        public virtual void ExecuteSchemaScript(string url, List<string> structure)
        {
            using (DbSession session = DbDrivers.Instance.Session(url))
            {
                using (DbCmd cmd = session.Command(""))
                {
                    for (var i = 0; i < structure.Count; ++i)
                    {
                        cmd.Sql = structure[i];
                        cmd.Execute();
                    }
                }
            }
        }

        ///<summary>
        /// Check model before generate migration script
        ///</summary>
        public virtual void CheckModel()
        {
            //Add indexes for all foreign keys   
            for (var i = 0; i < tables.Count; i++)
            {
                tables[i].CheckIndexesForFK();
            }
        }

        #endregion

        #region SQL


        ///<summary>
        /// Generate create script for database
        ///</summary>
        public List<string> CreateSQL(bool createTables = true, bool createIndexes = true, bool createFKs = true)
        {
            var structure = new List<string>();
            if (createTables)
            {
                for (var i = 0; i < tables.Count; i++)
                {
                    var table = tables[i]; 
                    table.CreateColumnsSQL(structure);
                    table.CreatePKSQL(structure);
                }

            }
            if (createIndexes)
            {
                for (var i = 0; i < tables.Count; i++)
                {
                    var table = tables[i];
                    table.CreateIndexesSQL(structure);
                }
            }
            if (createIndexes && createFKs)
            {
                for (var i = 0; i < tables.Count; i++)
                {
                    var table = tables[i];
                    table.CreateFKSQL(structure);
                }
            }
           
            return structure;
        }

        #endregion

    }
}
