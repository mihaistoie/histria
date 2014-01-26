using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbIndexes : KeyedCollection<string, DbIndex>
    {
        protected override string GetKeyForItem(DbIndex item)
        {
            return item.IndexName;
        }
    }
}
