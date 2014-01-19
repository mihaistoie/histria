using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Sikia.Db
{
    public class DbTran : IDisposable
    {
        private bool disposed = false;
        
        protected DbTransaction tran = null;
        protected DbSession session = null;
        
        private DbTran()
        {
        }
        
        public DbTran(DbSession session)
        {
            this.session = session;
        }
        public void Commit()
        {
            if (tran != null)
            {
                tran.Commit();
            }
        }

        // Implement IDisposable. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        // Implement IDisposable. 
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing && tran != null)
                {
                    // Dispose managed resources.
                    tran.Dispose();
                    tran = null;
                    session = null;
                }
                disposed = true;
            }

        }

        ~DbTran()
        {
            Dispose(false);
        }
    }
}
