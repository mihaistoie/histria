using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Db.Model
{
    public class DbIndex
    {
        #region Implementation
        protected string indexName;
        private string _indexFields;
        protected DbIndexColumns columns = new DbIndexColumns();
        #endregion

        #region Properties (IndexName/TableName/Unique)
        ///<summary>
        /// The name of index
        ///</summary
        public string IndexName
        {
            get
            {
                if (string.IsNullOrEmpty(indexName))
                {
                    var inb = new StringBuilder().Append(TableName);
                    for (var i = 0; i < columns.Count; ++i) 
                    {
                        var cc = columns[i];
                        inb.Append("_").Append(cc.ColumnName);
                        if (cc.Descending)
                            inb.Append("_d");

                    }
                    indexName = inb.ToString();

                }
                return indexName;
            }
            set
            {
                indexName = value;
            }
        }
        ///<summary>
        /// Index unique
        ///</summary
        public bool Unique { get; set; }
        ///<summary>
        /// TableName
        ///</summary
        public string TableName { get; set; }
        #endregion

        #region Index Columns

        ///<summary>
        /// Number of fields in index 
        ///</summary
        public int ColumnCount { get { return columns.Count;  } }

        ///<summary>
        /// Column by index
        ///</summary
        public DbIndexColumn ColumnByIndex(int index) { return columns[index]; }


        ///<summary>
        /// List of index fields (comma-separated) 
        ///</summary
        public string IndexFields
        {
            get
            {
                if (string.IsNullOrEmpty(_indexFields))
                {
                    var inb = new StringBuilder();
                    for (var i = 0; i < columns.Count; ++i)
                    {
                        var cc = columns[i];
                        inb.Append(cc.ColumnName.ToLower()).Append(",");
                    }
                    _indexFields = inb.ToString();
                }
                return _indexFields;
            }
        }

        ///<summary>
        /// Add a column to index
        ///</summary
        public void AddColumn(string columnName, bool descending = false)
        {
            columns.Add(new DbIndexColumn() { ColumnName = columnName, Descending = descending });
        }
        #endregion
        
        #region Schema SQL
        ///<summary>
        /// Generate create sql for the index
        ///</summary
        public void CreateSQL(StringBuilder sql)
        {
            sql.Append(string.Format("CREATE {0}INDEX {1} ON {2}(", (Unique ? "UNIQUE " : string.Empty), IndexName, TableName));

            for (var i = 0; i < columns.Count; ++i)
            {
                var ic = columns[i];
                if (i > 0) sql.Append(", ");
                sql.Append(string.Format("{0}{1}", ic.ColumnName, (ic.Descending ? " DESC" : string.Empty)));
            }
            sql.Append(")");
        }
        #endregion
    }





}


