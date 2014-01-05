using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sikia.DataBase;

namespace Sikia.Framework.DBModel
{
    public class DBStructure
    {

        protected DatabaseTranslator Translator { get; set; }
        public Dictionary<string, DBTable> Tables = new Dictionary<string, DBTable>();
        public virtual void Load(string databaseUrl)
        {
        }
        
    }

}
