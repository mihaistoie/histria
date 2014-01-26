using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbIndexColumns : KeyedCollection<string, DbIndexItem>
    {
        protected override string GetKeyForItem(DbIndexItem item)
        {
            return item.ColumnName;
        }
    }
}
