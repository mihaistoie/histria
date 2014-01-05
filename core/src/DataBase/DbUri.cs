using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.DataBase
{
    public class DbUri
    {
        #region Private members
        private DbProtocol protocol = DbProtocol.unknown;
        private string squery = "";
        private string sserver = "";
        private string sdatabase = "";
        private Dictionary<string, string> query = new Dictionary<string,string>();
        #endregion

        #region Constructor
        public DbUri(string url)
        {
            int index = url.IndexOf("://");
            if (index <= 0)
                throw DbUriError(string.Format("Invalid DbUri format : {0}", url));
            protocol = DbServices.StringToProtocol(url.Substring(0, index).ToLower());
            if (protocol == DbProtocol.unknown)
            {
                throw DbUriError(string.Format("Unknown database protocol : {0}.", url.Substring(0, index).ToLower()));
            }
            string tmp = url.Substring(index + 3);
            index = tmp.IndexOf("?");
            if (index >= 0)
            {
                squery = tmp.Substring(index + 1);
                tmp = tmp.Substring(0, index);
                foreach (var ss in squery.Split('&'))
                {
                    var ii = ss.IndexOf('=');
                    if (ii > 0)
                    {
                        query.Add(ss.Substring(0, ii), ss.Substring(ii+1));
                    }
                }
            }
            index = tmp.IndexOf("/");
            if (index <= 0)
                 throw DbUriError(string.Format("Invalid DbUri format : {0}", url));
            sserver = tmp.Substring(0, index);
            sdatabase = tmp.Substring(index + 1);
        }
        #endregion

        #region Properties
        public string ServerAddress { get { return sserver; } }
        public string DatabaseName { get { return sdatabase; } }
        public DbProtocol Protocol { get { return protocol; } }
        public Dictionary<string, string> Query { get { return query; } }
        #endregion

        Exception DbUriError(string msg)
        {
            return new ArgumentException(msg);
        }
    }
}
