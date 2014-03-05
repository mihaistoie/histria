using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Db.Model
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

        #region Columns

        /////<summary>
        ///// List of column
        /////</summary>
        public DbColumns Columns
        {
            get
            {
                return columns;
            }
        }

        /////<summary>
        ///// Table columns
        /////</summary>
        //public DbColumns Columns { get { return columns; } }

        ///<summary>
        /// Column By Name
        ///</summary>
        public DbColumn ColumnByName(string columnName) { return columns[columnName.ToLower()]; }

        ///<summary>
        /// Returns true if the table has columnName
        ///</summary>
        public bool ContainsColumn(string columnName) { return columns.Contains(columnName.ToLower()); }

        ///<summary>
        /// Add an index
        ///</summary>
        public void AddColumn(DbColumn column)
        {
            column.TableName = TableName;
            columns.Add(column);
        }

        ///<summary>
        /// Number of columns 
        ///</summary>
        public int ColumnCount { get { return columns.Count; } }
        #endregion

        #region Indexes
        /////<summary>
        ///// Table indexes
        /////</summary>
        //public DbIndexes Indexes { get { return indexes; } }

        ///<summary>
        /// Index By Name
        ///</summary>
        public DbIndex IndexByName(string indexName) { return indexes[indexName.ToLower()]; }

        ///<summary>
        /// Returns true if the table has indexName
        ///</summary>
        public bool ContainsIndex(string indexName) { return indexes.Contains(indexName.ToLower()); }

        ///<summary>
        /// Add an index
        ///</summary>
        public void AddIndex(DbIndex index)
        {
            index.TableName = TableName;
            indexes.Add(index);
        }

        ///<summary>
        /// Number of indexes 
        ///</summary>
        public int IndexCount { get { return indexes.Count; } }

        #endregion

        ///<summary>
        /// Foreign keys
        ///</summary>
        public List<DbFk> ForeignKeys { get { return foreignKeys; } }

        #region Properties
        ///<summary>
        /// Table name
        ///</summary
        public string TableName { get; set; }
        #endregion

        #region Schema
        public virtual void CreateColumnsSQL(List<string> structure) { }
        public virtual void CreateIndexesSQL(List<string> structure) { }
        public virtual void CreateFKSQL(List<string> structure) { }
        public virtual void CreatePKSQL(List<string> structure) { }
        ///<summary>
        /// Returns true if the table contains an index 
        /// that starts with "fields" (comma-separated) ?
        ///</summary
        public virtual bool HasIndexForFields(string fields)
        {
            var ci = fields.ToLower();
            if (!ci.EndsWith(","))
            {
                ci += ",";
            }

            for (var i = 0; i < indexes.Count; i++)
            {
                if (indexes[i].IndexFields.StartsWith(ci))
                {
                    return true;
                }
            }
            return false;
        }
        ///<summary>
        /// Vreate an index for evry foreign key
        ///</summary
        public virtual void CheckIndexesForFK()
        {
            var self = this;
            for (var i = 0; i < foreignKeys.Count; i++)
            {
                var fk = foreignKeys[i];
                if (!HasIndexForFields(fk.IndexFields))
                {
                    var ii = new DbIndex() { TableName = self.TableName };
                    for (var j = 0; j < fk.Columns.Count; j++)
                        ii.AddColumn(fk.Columns[j].ColumnName);
                    indexes.Add(ii);
                }
            }
        }
        #endregion
    }
}
