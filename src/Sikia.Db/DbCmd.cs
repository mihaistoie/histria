using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Sikia.Utils;

namespace Sikia.Db
{
    public class DbCmd : IDisposable
    {
        private bool disposed = false;
        protected DbCommand cmd = null;
        protected DbSession session = null;
        protected DbTran transaction = null;
        protected DbCmdParameters parameters = null;
        protected string SQL()
        {
            //TODO remplace params with values
            return Sql;
        }

        protected virtual void InternalExecute()
        {
        }

        protected virtual Object InternalExecuteScalar()
        {
            return null;
        }
       

        private DbCmd()
        {
        }

        public DbCmd(DbSession dbSession, DbTran dbTransaction)
        {
            session = dbSession;
            transaction = dbTransaction;
        }

        public DbCmdParameters Parameters
        {
            get
            {
                if (parameters == null)
                    parameters = new DbCmdParameters();
                return parameters;
            }
        }
        public virtual void Clear()
        {
            if (parameters != null)
                parameters.Clear();

        }

        public string Sql { get; set; }


        public void Execute()
        {
            DateTime start = DateTime.Now;

            try
            {
                InternalExecute();

            }
            finally
            {
                TimeSpan interval = start - DateTime.Now;
                Logger.Info(DbServices.SQL, SQL(), interval.TotalMilliseconds);
            }
        }

        public Object ExecuteScalar()
        {
            DateTime start = DateTime.Now;
            try
            {
                return InternalExecuteScalar();
            }
            finally
            {
                TimeSpan interval = start - DateTime.Now;
                Logger.Info(DbServices.SQL, SQL(), interval.TotalMilliseconds);
            }
        }

        // Implement IDisposable. 
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing && cmd != null)
                {
                    // Dispose managed resources.
                    cmd.Dispose();
                }
                disposed = true;
            }
        }
        ~DbCmd()
        {
            Dispose(false);
        }
    }
}
