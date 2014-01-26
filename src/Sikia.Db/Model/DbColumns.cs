using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbColumns : KeyedCollection<string, DbColumn>
    {
        public bool TryGetValue(string name, out DbColumn column)
        {
            column = null;
            if (Contains(name))
            {
                column = this[name];
                return true;
            }
            return false;
        }
        protected override string GetKeyForItem(DbColumn item)
        {
            return item.ColumnName;
        }
    }
}



