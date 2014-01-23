using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Sikia.Db.SqlServer
{
    public class MsSqlTranslator : DbTranslator
    {
        #region SQL's

        public string SQL_READ_COMMITTED_SNAPSHOT()
        {
            return "ALTER DATABASE {0} SET READ_COMMITTED_SNAPSHOT ON";
        }
       
        public string SQL_DatabaseExists()
        {
            return "SELECT count(*) FROM master.dbo.sysdatabases where name = @name";
        }

        public string SQL_CreateDatabase()
        {
            return  "CREATE DATABASE {0}";
        }

        public string SQL_DropDatabase()
        {
            return "DROP DATABASE {0}";
        }

        #endregion


        public override List<string> Tables(DbSession session)
        {
            List<string> res = new List<string>();
           
            /*
            SqlCommand command = new SqlCommand(SQL_TableList(), (SqlConnection)connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    res.Add(reader.GetString(0));
                }
            }
            reader.Close();
             * */
            return res;
        }
    }
        
}
