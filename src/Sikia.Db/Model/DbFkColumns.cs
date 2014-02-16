using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbFkColumns : KeyedCollection<string, DbFkItem>
    {
        protected override string GetKeyForItem(DbFkItem item)
        {
            return item.ColumnName.ToLower();
            
        }
        public DbFkColumns() : 
            base(StringComparer.OrdinalIgnoreCase) {
        }
    }
}
