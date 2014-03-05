using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Histria.Db.Model;

namespace Histria.Db.SqlServer.Model
{
    public class MsSqlColumn : DbColumn
    {
        public MsSqlColumn()
            : base()
        {
        }
        private string SQLType()
        {
            switch (DbType)
            {
                case "bigint":
                case "bit":
                case "smallint":
                case "int":
                case "money":
                case "smallmoney":
                case "tinyint":
                case "float":
                case "real":
                case "uniqueidentifier":
                case "date":
                case "datetime":
                case "time":
                case "smalldatetime":
                    return DbType;

                case "decimal":
                case "numeric":
                    if (Precision == 0 && Scale == 0)
                        return DbType;
                    if (Scale == 0)
                        return string.Format("{0}({1})", DbType, Precision);
                    else
                        return string.Format("{0}({1},{2})", DbType, Precision, Scale);

                case "varchar":
                case "nvarchar":
                case "varbinary":
                    if (Size > 0)
                        return string.Format("{0}({1})", DbType, Size);
                    else
                        return string.Format("{0}(max)", DbType);
                case "char":
                case "nchar":
                    return string.Format("{0}({1})", DbType, Size);

                case "text": return "varchar(max)";
                case "ntext": return "nvarchar(max)";

                default:
                    return DbType;
            }
        }
        private string SQLDefault()
        {
            return string.Empty;
        }
        private string FieldDef()
        {
            var notNull = Nullable ? string.Empty : " not null";
            return string.Format("{0} {1}{2}{3}", ColumnName, SQLType(), notNull, SQLDefault());
        }

        public override void CreateSQL(StringBuilder sql)
        {
            sql.Append(FieldDef());
        }
    }
}
