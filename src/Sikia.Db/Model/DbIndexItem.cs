using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbIndexItem
    {
        public string ColumnName { get; set; }
        public bool Descending { get; set; }
    }
}
