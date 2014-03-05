using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Histria.Db.SqlServer
{
    public class MsSqlTransaction : DbTran
    {
        public SqlTransaction Transaction
        {
            get
            {
                return (tran as SqlTransaction);
            }
        }

        public MsSqlTransaction(DbSession session)
            : base(session)
        {
            this.tran = (session as MsSqlSession).Connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

    }
}
