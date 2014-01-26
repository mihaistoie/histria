using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbIndex
    {
        protected DbIndexColumns columns = new DbIndexColumns();
        public DbIndexColumns Columns { get { return columns; } }
        public string IndexName { get; set; }
        public bool Unique { get; set; }
        public void AddColumn(string columnName, bool descending = false) 
        {
            columns.Add(new DbIndexItem() { ColumnName = columnName, Descending = descending});
        }
    }
    
    


    
}


