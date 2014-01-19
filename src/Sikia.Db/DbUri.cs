using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Sikia.Db
{
    ///<summary>
    /// Database uri parser 
    /// var db = new DbUri("mssql://msdbs/customers?scema=dbo");
    ///</summary>
    public class DbUri
    {
        #region Private members
        private Dictionary<string, string> query = new Dictionary<string,string>();
        void parseUri(string url)
        {
            int index = url.IndexOf("://");
            if (index <= 0)
                throw DbUriError(string.Format("Invalid DbUri format : {0}", url));
            Protocol = DbServices.Str2DbProtocol(url.Substring(0, index).ToLower());
            if (Protocol == DbProtocol.unknown)
            {
                throw DbUriError(string.Format("Unknown database protocol : {0}.", url.Substring(0, index).ToLower()));
            }
            string tmp = url.Substring(index + 3);
            index = tmp.IndexOf("?");
            if (index >= 0)
            {
                var squery = tmp.Substring(index + 1);
                tmp = tmp.Substring(0, index);
                foreach (var ss in squery.Split('&'))
                {
                    var ii = ss.IndexOf('=');
                    if (ii > 0)
                    {
                        query.Add(ss.Substring(0, ii), WebUtility.HtmlDecode(ss.Substring(ii + 1)));
                    }
                }
            }
            index = tmp.IndexOf("/");
            if (index <= 0)
                throw DbUriError(string.Format("Invalid DbUri format : {0}", url));
            ServerAddress = tmp.Substring(0, index);
            DatabaseName = tmp.Substring(index + 1);
        }
        #endregion

        #region Constructor
        public DbUri(string url)
        {
             parseUri(url);
        }
        #endregion

        #region Properties
        ///<summary>
        /// Server address
        ///</summary>
        public string ServerAddress { get; set; }
        ///<summary>
        /// Name of database 
        ///</summary>
        public string DatabaseName { get; set; }
        ///<summary>
        /// Database protocol: mssql / ora ... 
        ///</summary>
        public DbProtocol Protocol { get; set; }
        ///<summary>
        /// Query part of url
        ///</summary>
        public Dictionary<string, string> Query { get { return query; } }
        #endregion

        Exception DbUriError(string msg)
        {
            return new ArgumentException(msg);
        }
    }
}
