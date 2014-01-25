using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Sikia.Sys;

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

        protected virtual DbDataReader InternalExecuteReader()
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
                TimeSpan interval = DateTime.Now - start;
                Logger.Info(Logger.SQL, SQL(), interval.TotalMilliseconds);
            }
        }

        public DbDataReader ExecuteReader()
        {
            DateTime start = DateTime.Now;

            try
            {
                return InternalExecuteReader();

            }
            finally
            {
                TimeSpan interval = DateTime.Now - start;
                Logger.Info(Logger.SQL, SQL(), interval.TotalMilliseconds);
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
                TimeSpan interval = DateTime.Now - start;
                Logger.Info(Logger.SQL, SQL(), interval.TotalMilliseconds);
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
