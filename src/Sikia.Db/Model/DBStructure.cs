using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.Model
{
    public class DbStructure
    {
        protected Dictionary<string, DBTable> tables = new Dictionary<string, DBTable>();
        public virtual void CreateDatabase(string url) 
        {
        }

        public virtual bool DatabaseExists(string url)
        {
            return true;
        }

        public virtual void DropDatabase(string url)
        {
        }
        public virtual void Load(string url)
        {
            tables.Clear();
        }

    }
}
