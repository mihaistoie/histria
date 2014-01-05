using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Sikia.DataBase
{
    public class MsSqlTranslator : DatabaseTranslator
    {
        #region SQL's

        private string SQL_TableList()
        {
            return "SELECT TABLE_NAME FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = ? and Table_Type='BASE TABLE' ORDER BY TABLE_NAME";
        }

        public string SQL_DatabaseExists()
        {
            return "SELECT count(*) FROM master.dbo.sysdatabases where name = ?";
        }
        #endregion


        public override List<string> Tables(DbConnection connection)
        {
            List<string> res = new List<string>();
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
            return res;
        }
    }
        
}
