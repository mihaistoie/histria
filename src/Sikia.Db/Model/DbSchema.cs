using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbSchema
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
            foreach (var table in tables.Values)
            {
                table.CheckIndexesForFK();
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
                foreach (var table in tables.Values)
                {
                    table.CreateColumnsSQL(structure);
                    table.CreatePKSQL(structure);
                }

            }
            if (createFKs)
            {
                foreach (var table in tables.Values)
                {
                    table.CreateFKSQL(structure);
                }
            }
            if (createIndexes)
            {
                foreach (var table in tables.Values)
                {
                    table.CreateIndexesSQL(structure);
                }
            }
            return structure;
        }

        #endregion

    }
}
