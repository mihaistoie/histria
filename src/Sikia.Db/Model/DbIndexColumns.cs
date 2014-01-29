using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbIndexColumns : KeyedCollection<string, DbIndexColumn>
    {
        protected override string GetKeyForItem(DbIndexColumn item)
        {
            return item.ColumnName.ToLower();
        }
    }
}
