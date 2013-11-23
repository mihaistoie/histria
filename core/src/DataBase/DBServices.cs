using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sikia.DataBase
{
    public static class DBServices
    {
        #region Provider
        public static string DBProviderToString(DBProvider val)
        {
            return Enum.GetName(typeof(DBProvider), val);
        }
        public static DBProvider StringToBProvider(string val)
        {
            DBProvider[]allValues =  (DBProvider[])Enum.GetValues(typeof(DBProvider));
            Type tdp = typeof(DBProvider);
            for (int i = 0; i < allValues.Length; i++)
            {
                if (val == Enum.GetName(tdp, allValues[i])) 
                    return allValues[i]; 
            }
            return allValues[0]; ;
        }
        #endregion
        #region Databaseurl 
        public static string ParseDataBaseUrl()
        {
            // provider://serveraddress/dbname
            //return String.Format("{0}://{1}/{2}", Provider(), System.Web.HttpUtility.UrlEncode(ServerAddress),
            //    HttpUtility.UrlEncode(DatabaseName));
            return "";
        }
        #endregion
    }
}
