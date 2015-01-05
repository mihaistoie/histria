using Histria.Db.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Histria.Model.Db
{
    /// <summary>
    /// Database extention for Model Manager 
    /// </summary>
    public static class ModelDbExtension
    {
        #region Implementation
        private static void Property2Column(PropInfoItem pi, DbColumn col,  DbTable tt)
        {
            col.ColumnName = pi.DbName;
            col.Nullable = pi.IsMandatory || tt.PK.Contains(col.ColumnName, StringComparer.OrdinalIgnoreCase);
            //col.Type

        }

        private static void LoadColumns(ClassInfoItem ci, DbTable tt, DbSchema schema)
        {
            List<ClassInfoItem> list = new List<ClassInfoItem>() { ci };
            list.AddRange(ci.Descendants);
            foreach (ClassInfoItem cii in list)
            {
                foreach (PropInfoItem pi in cii.Properties)
                {
                    if (!pi.IsPersistent || tt.ContainsColumn(pi.DbName))  continue;
                    DbColumn c = schema.Column();
                    Property2Column(pi, c, tt);
                    tt.AddColumn(c);
                }
            }
        }

        private static void LoadPK(ClassInfoItem ci, DbTable tt)
        {
            foreach (KeyItem key in ci.Key.Items)
            {
                PropInfoItem pi = ci.Properties[key.Property];
                tt.PK.Add(pi.DbName);
            }
        }

        private static void LoadIndexes(ClassInfoItem ci, DbTable tt)
        {
        }

        private static void LoadFKs(ClassInfoItem ci, DbTable tt)
        {
        }

        private static void LoadTable(ClassInfoItem ci, DbTable tt, DbSchema schema)
        {
            // Load columns 
            ClassInfoItem pci = ci.GetTopPersistentAscendent();
            LoadPK(ci, tt);
            LoadColumns(ci, tt, schema);
            LoadIndexes(ci, tt);
            LoadFKs(ci, tt);

        }

        #endregion

        /// <summary>
        /// Generate database schema from Business model   
        /// </summary>
        public static DbSchema Schema(this ModelManager model)
        {
            DbSchema schema = new DbSchema();
            foreach (ClassInfoItem ci in model.Classes)
            {
                if (ci.Static) continue;
                if (!ci.IsPersistent) continue;
                if (schema.ContainsTable(ci.DbName)) continue;
                DbTable tt = new DbTable();
                tt.TableName = ci.DbName;
                schema.Tables.Add(tt);
                LoadTable(ci, tt, schema);
            }
            return schema;
        }

    }
}
