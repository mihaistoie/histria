using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbIndex
    {
        protected IndexColumns columns = new IndexColumns();
        public IndexColumns Columns { get { return columns; } }
        public string IndexName { get; set; }
        public bool Unique { get; set; }
        public void AddColumn(string columnName, bool descending = false) 
        {
            columns.Add(new DbIndexItem() { ColumnName = columnName, Descending = descending});
        }
    }
    
    public class IndexColumns : KeyedCollection<string, DbIndexItem>
    {
        protected override string GetKeyForItem(DbIndexItem item)
        {
            return item.ColumnName;
        }
    }


    public class DbIndexes : KeyedCollection<string, DbIndex>
    {
        protected override string GetKeyForItem(DbIndex item)
        {
            return item.IndexName;
        }
    }
}


