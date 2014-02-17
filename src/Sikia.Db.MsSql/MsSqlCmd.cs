using Sikia.Sys;
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
        private static SqlDbType SqlType(DataTypes type)
        {
            switch (type)
            {
                case DataTypes.uuid:
                    return SqlDbType.UniqueIdentifier;
                case DataTypes.Int:
                    return SqlDbType.Int;
                case DataTypes.BigInt:
                    return SqlDbType.BigInt;
                case DataTypes.Bool:
                    return SqlDbType.Bit;
                case DataTypes.Enum:
                    return SqlDbType.SmallInt;
                case DataTypes.String:
                    return SqlDbType.VarChar;
                case DataTypes.Number:
                    return SqlDbType.Decimal;
                case DataTypes.Currency:
                    return SqlDbType.Money;
                case DataTypes.Date:
                    return SqlDbType.Date;
                case DataTypes.Time:
                    return SqlDbType.Time;
                case DataTypes.DateTime:
                    return SqlDbType.DateTime;
                case DataTypes.Memo:
                    return SqlDbType.Text;
                case DataTypes.Binary:
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

        protected override DbDataReader InternalExecuteReader()
        {
            PrepareExecute();
            return Command.ExecuteReader();
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
