using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace Sikia.Db
{
    public class DbSession : IDisposable
    {
        private bool disposed = false;
        protected DbConnection db = null;
        protected string url = "";

        public virtual void Open() { }

        // Implement IDisposable. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing && db != null)
                {
                    // Dispose managed resources.
                    db.Dispose();
                }
             }
        }
        ~DbSession()
        {
            Dispose(false);
        }
    }
}


