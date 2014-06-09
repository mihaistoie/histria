using Histria.Db.Model;
using Histria.Sys;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Histria.Db.Generators
{
    public class ClassTranslator
    {
        static private string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string Camelize(string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower()).Replace("_", "");
        }

        public string PrimaryKeyAttribute(DbTable table)
        {
            if (table.PK != null && table.PK.Count > 0)
            {
                string res = "", stag = "";
                foreach (var s in table.PK)
                {
                    res = res + stag + ColumnName2PropertyName(table.TableName, s);
                    stag = ",";
                }
                return string.Format("[PrimaryKey(\"{0}\")]", res);
            }
            return null;
        }

    
        public string TableName2ClassName(string tableName)
        {
            return Camelize(tableName);
        }

        public string DataTypeAttribute(DbColumn col)
        {
            switch (col.Type)
            {
                case DataTypes.String:
                    return string.Format("[DtString(Maxlength={0})]", col.Size);
                default:
                    return null;
            }
        }
        public string DefaultAttribute(DbColumn col)
        {
            if (!col.Nullable)
                return "[Default(Required=true)]";
            return null;
        }

        public string ColumnType(DbColumn col)
        {
            switch (col.Type)
            {
                case DataTypes.uuid:
                    return "Guid";
                case DataTypes.SmallInt:
                    return "int";
                case DataTypes.Int:
                    return "int";
                case DataTypes.BigInt:
                    return "long";
                case DataTypes.Bool:
                    return "bool";
                case DataTypes.Number:
                    return "decimal";
                case DataTypes.String:
                    return "string";
                default:
                    return "???";

                /*
                   case DataTypes.Enum,       //smallint
                   case DataTypes.String,     //varchar or nvarchar
                   case DataTypes.Currency,   //Money (decimal(19.4)) 
                   case DataTypes.Date,       //Date
                   case DataTypes.Time,       //time
                   case DataTypes.DateTime,   //DateTime
                   case DataTypes.Memo,       //Text or Ntext 
                   case DataTypes.Binary             
                 */
            }

        }


        public string ColumnName2PropertyName(string tableName, string coumnName)
        {
            return Camelize(coumnName);
        }
        public string ColumnName2PropertyTitle(string tableName, string coumnName)
        {
            return string.Empty;
        }
        public string ColumnName2PropertyDescription(string tableName, string coumnName)
        {
            return string.Empty;
        }


    }
}
