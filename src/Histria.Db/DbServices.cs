using Histria.Json;
using Histria.Sys;
using System;


namespace Histria.Db
{
    public static class DbServices
    {
        #region Provider
        ///<summary>
        /// Convert DbProtocol to string 
        ///</summary>
        public static string DbProtocol2Str(DbProtocol val)
        {
            return Enum.GetName(typeof(DbProtocol), val);
        }

        ///<summary>
        /// Convert string to DbProtocol 
        ///</summary>
        public static DbProtocol Str2DbProtocol(string val)
        {
            DbProtocol[]allValues =  (DbProtocol[])Enum.GetValues(typeof(DbProtocol));
            Type tdp = typeof(DbProtocol);
            for (int i = 0; i < allValues.Length; i++)
            {
                if (val == Enum.GetName(tdp, allValues[i])) 
                    return allValues[i]; 
            }
            return DbProtocol.unknown;
        }
        public static DbProtocol Url2Protocol(string url)
        {
            return Str2DbProtocol(url.Substring(0, url.IndexOf(':')));

        }


        #endregion
    }
}
