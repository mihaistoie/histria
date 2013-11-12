using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Sikia.DataBase;

namespace Sikia.Framework.DBModel
{
    public class DBStructure
    {
        public Dictionary<string, DBTable> Tables = new Dictionary<string, DBTable>();
        public void LoadFromDataBase(SqlConnection connection, DatabaseTranslator translator)
        {
            /* */
        
        }
        
    }
}
