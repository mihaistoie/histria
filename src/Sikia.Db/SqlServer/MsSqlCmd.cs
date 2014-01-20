using Sikia.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Sikia.Db.SqlServer
{
    public class MsSqlCmd : DbCmd
    {
        private SqlCommand Command
        {
            get
            {
                return cmd as SqlCommand;
            }
        }

        public MsSqlCmd(DbSession dbSession, DbTran dbTransaction)
            : base(dbSession, dbTransaction)
        {
        }
        // Create DbCommand 
        private void CheckCommand()
        {
            if (cmd == null)
            {
                MsSqlSession sess = (session as MsSqlSession);
                SqlTransaction tran = null;
                if (transaction != null)
                {
                    tran = (transaction as MsSqlTransaction).Transaction;
                }
                cmd = new SqlCommand();
                cmd.Connection = sess.Connection;
                if (tran != null)
                    cmd.Transaction = tran;
            }
        }
        private static SqlDbType SqlType(DbType type)
        {
            switch (type)
            {
                case DbType.uuid:
                    return SqlDbType.UniqueIdentifier;
                case DbType.Int:
                    return SqlDbType.Int;
                case DbType.BigInt:
                    return SqlDbType.BigInt;
                case DbType.Bool:
                    return SqlDbType.Bit;
                case DbType.Enum:
                    return SqlDbType.SmallInt;
                case DbType.Varchar:
                    return SqlDbType.VarChar;
                case DbType.Float:
                    return SqlDbType.Float;
                case DbType.Currency:
                    return SqlDbType.Money;
                case DbType.Date:
                    return SqlDbType.Date;
                case DbType.Time:
                    return SqlDbType.Time;
                case DbType.DateTime:
                    return SqlDbType.DateTime;
                case DbType.Memo:
                    return SqlDbType.Text;
                case DbType.Binary:
                    return SqlDbType.Binary;
                default:
                    throw new NotImplementedException(StrUtils.TT("Invalid type"));
            }
        }

        private void DeclareParameter(DbCmdParameter p)
        {
            Command.Parameters.Add(p.Name, MsSqlCmd.SqlType(p.Type), p.Size).Value = p.Value;
        }

        private void DeclareParameters()
        {
            if (parameters != null)
            {
                foreach (var p in parameters)
                    DeclareParameter(p);
            }
        }

        private void SetParameters()
        {
            if (parameters != null)
            {
                foreach (var p in parameters)
                    Command.Parameters[p.Name].Value = p.Value;
            }
        }

        private void PrepareExecute()
        {
            CheckCommand();
            if (string.IsNullOrEmpty(Command.CommandText) || Command.CommandText != Sql)
            {
                Command.Parameters.Clear();
                Command.CommandText = Sql;
                DeclareParameters();
                if ((parameters != null) && parameters.Count() > 0)
                {
                    Command.Prepare();
                }
            }
            else
            {
                SetParameters();
            }
    
        }
        public override void Clear()
        {
            base.Clear();
            if (Command != null)
            {
                Command.Parameters.Clear();
            }
        }

        protected override void InternalExecute()
        {
            PrepareExecute();
            Command.ExecuteNonQuery();
        }
        
        protected override Object InternalExecuteScalar()
        {
            PrepareExecute();
            return Command.ExecuteScalar();
        }
       

    }
}
