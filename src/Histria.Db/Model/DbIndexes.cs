﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Histria.Db.Model
{
    public class DbIndexes : KeyedCollection<string, DbIndex>
    {
        protected override string GetKeyForItem(DbIndex item)
        {
            return item.IndexName.ToLower();
        }
        public DbIndexes() :
            base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}
