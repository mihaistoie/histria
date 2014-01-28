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

        #region Schema
        public virtual void CreateColumnsSQL(List<string> structure) { }
        public virtual void CreateIndexesSQL(List<string> structure) { }
        public virtual void CreateFKSQL(List<string> structure) { }
        public virtual void CreatePKSQL(List<string> structure) { }
        ///<summary>
        /// This table contains an index that starts with "fields" (comma separated) ?
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
                if (indexes[i].IndexName.StartsWith(ci))
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
