using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Histria.Db.Model;
using Histria.Sys;

namespace Histria.Db.Generators
{
    public class Generator
    {
        #region Internals
        private StringBuilder _sb;
        private ClassTranslator _ct = new ClassTranslator();
        private int _indentCount = 0;

        private static string _indent = "\t";
        private void AppendLine(string value)
        {
            string indent = string.Concat(Enumerable.Repeat(_indent, _indentCount));
            _sb.AppendLine(string.Format("{0}{1}", indent, value));
        }

        private void AppendEmtyLine()
        {
            _sb.AppendLine();
        }

        #endregion

        #region Class generation
        private void GenerateUsing()
        {
            _indentCount = 0;
            AppendLine("using System;");
            AppendLine("using Histria.Core;");
            AppendLine("using Histria.Model;");
            AppendLine("using Histria.Sys;");

        }


        private void Column2Class(DbTable table, DbColumn col)
        {
            string title = _ct.ColumnName2PropertyTitle(table.TableName, col.ColumnName);
            string description = _ct.ColumnName2PropertyDescription(table.TableName, col.ColumnName);
            if (!string.IsNullOrEmpty(title))
            {
                AppendLine(string.Format(@"[Display(""{0}"", Description = ""{1}"")]", title, description));
            }
            string a = _ct.DefaultAttribute(col);
            if (!string.IsNullOrEmpty(a))
            {
                AppendLine(a);
            }
            a = _ct.DataTypeAttribute(col);
            if (!string.IsNullOrEmpty(a))
            {
                AppendLine(a);
            }
            string propName = _ct.ColumnName2PropertyName(table.TableName, col.ColumnName);
            if (propName != col.ColumnName)
                AppendLine(string.Format(@"[Persistent(PersistentName = ""{0}"")]", col.ColumnName));

            AppendLine(string.Format("public virtual {0} {1} {{ get; set; }}", _ct.ColumnType(col), _ct.ColumnName2PropertyName(table.TableName, col.ColumnName)));
            AppendEmtyLine();
 
        }

        private void BuildClass(DbTable table)
        {
            string className = _ct.TableName2ClassName(table.TableName);
            _sb = new StringBuilder();
            //Using 
            GenerateUsing();
            AppendEmtyLine();

            // Namespace
            AppendLine(string.Format("namespace {0}", this.OutputNamespace));
            AppendLine("{");
            _indentCount++;
            // Class
            AppendLine(string.Format(@"[Db(""{0}"")]", table.TableName));
            string pk = _ct.PrimaryKeyAttribute(table);
            if (!string.IsNullOrEmpty(pk))
            {
                AppendLine(pk);
            }
            
            AppendLine(string.Format("public partial class {0} : InterceptedObject ", className));
            AppendLine("{");

            _indentCount++;
            foreach (DbColumn col in table.Columns)
            {
                Column2Class(table, col);
            }
            _indentCount--;
            AppendLine("}");
            _indentCount--;
            AppendLine("}");
        }

        #endregion

        #region Properties
        public string OutputFolder { get; set; }
        public string OutputNamespace { get; set; }
        public string DatabaseUrl { get; set; }
        #endregion

        #region Constructor
        public Generator(string databaseUrl, string repertoireDest, string nameSpace)
        {
            OutputFolder = repertoireDest;
            OutputNamespace = nameSpace;
            DatabaseUrl = databaseUrl;
        }
        #endregion

        public void Generate()
        {

            if (string.IsNullOrEmpty(DatabaseUrl))
            {
                throw new Exception(L.T("Database is empty"));
            }

            DbSchema ss = DbDrivers.Schema(DbServices.Url2Protocol(DatabaseUrl));

            //Load database _structure 
            ss.Load(DatabaseUrl);
            foreach (var table in ss.Tables)
            {
                GenerateClass(table);
            }
        }

        #region méthodes de génération classes
        private void GenerateClass(DbTable table)
        {
            BuildClass(table);
            string className = _ct.TableName2ClassName(table.TableName);
            DirectoryInfo dd = new DirectoryInfo(this.OutputFolder);
            if (!dd.Exists)
                dd.Create();

            using (TextWriter writer = new StreamWriter(Path.Combine(this.OutputFolder, className + ".cs")))
            {
                writer.Write(_sb.ToString());
            }

        }

        #endregion
    }
}
