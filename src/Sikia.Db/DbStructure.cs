using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db
{
    public class DbStructure
    {
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
    }
}
