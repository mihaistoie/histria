using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace Histria.Db
{
    public abstract class DbSession : IDisposable
    {
        private bool disposed = false;
        protected DbConnection db = null;
        protected string url;


        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                setUrl(value);
            }
        }
        protected virtual void setUrl(string value) 
        {
            url = value;
        }

        public virtual void Open() { }

        // Implement IDisposable. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual DbTran BeginTransaction()
        {
            return new DbTran(this);
        }

        public virtual DbCmd Command(string sql)
        {
            DbCmd cmd = new DbCmd(this, null);
            cmd.Sql = sql;
            return cmd;
        }

        public virtual DbCmd Command(string sql, DbTran transaction)
        {
            DbCmd cmd = new DbCmd(this, transaction);
            cmd.Sql = sql;
            return cmd;
        }

        public virtual DbCmd Command(DbTran transaction)
        {
            return new DbCmd(this, transaction);
        }

        // Implement IDisposable. 
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing && db != null)
                {
                    // Dispose managed resources.
                    db.Dispose();
                    db = null;
                }
                disposed = true;
            }
        }
        ~DbSession()
        {
            Dispose(false);
        }
    }
}


