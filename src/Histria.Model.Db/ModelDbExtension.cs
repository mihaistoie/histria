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
        private static void LoadColumns(ClassInfoItem ci, DbTable tt)
        {
        }

        private static void LoadPK(ClassInfoItem ci, DbTable tt)
        {
        }

        private static void LoadIndexes(ClassInfoItem ci, DbTable tt)
        {
        }

        private static void LoadFKs(ClassInfoItem ci, DbTable tt)
        {
        }

        private static void LoadTable(ClassInfoItem ci, DbTable tt)
        {
            // Load columns 

            LoadPK(ci, tt);
            LoadColumns(ci, tt);
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
                LoadTable(ci, tt); 
            }
            return schema;
        }

    }
}
