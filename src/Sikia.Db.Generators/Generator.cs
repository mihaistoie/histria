using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sikia.Db.Model;

namespace Sikia.Db.Generators
{
    public class Generator
    {
        #region proprietes

        public String RepertoireDest { get; set; }
        public DbSchema Schema { get; set; }
        public String NameSpaceDest { get; set; }
        private StringBuilder m_sb;
        private string m_Indent = String.Empty;
        private string FileName { get; set; }

        private string m_AppDir;
        public string AppDir
        {
            get
            {
                if (String.IsNullOrEmpty(m_AppDir))
                {
                    var a = Assembly.GetExecutingAssembly();
                    Uri u = new Uri(a.CodeBase);
                    string exePath = u.LocalPath;
                    string root = Path.GetDirectoryName(exePath);
                    m_AppDir = Path.Combine(root);
                }
                return m_AppDir;
            }
        }
        #endregion

        #region constructeurs

        public Generator(DbSchema schema, string repertoireDest, string nameSpace)
        {
            Schema = schema;
            RepertoireDest = repertoireDest;
            NameSpaceDest = nameSpace;
        }
        #endregion

        public void Generate()
        {
            foreach (var table in Schema.Tables)
            {
                GenerateClass(table);
            }
        }

        #region méthodes de génération classes
        private void GenerateClass(DbTable table)
        {
            Initial();
            WriteClass(table);

            string strGen = m_sb.ToString();
            if (strGen != null)
            {
                SaveFile(strGen, table.TableName);
            }
        }

        private void WriteClass(DbTable table)
        {
            // Using
            AppendLine("using System;");
            AppendLine("using Sikia.Core;");
            AppendLine("using Sikia.Model;");
            AppendLine("using Sikia.Sys;");
            AppendLine("using Sikia.Json;");
            // Namespace
            AppendLine();
            AppendIndent();
            AppendLine("namespace {0}", this.NameSpaceDest);
            AppendLine("{");
            AppendIndent();
            // Class
            Indent();
            AppendLine("public partial class {0} : InterceptedObject ", table.TableName);
            AppendLine("{");
            Indent();
            foreach (var col in table.Columns)
            {
                AppendLine("public virtual {0} {1} {{ get; set; }}", col.Type, col.ColumnName);
            }
            UnIndent();
            AppendLine("}");
            UnIndent();
            AppendLine("}");
        }

        private void Initial()
        {
            m_sb = new StringBuilder();
        }

        private void SaveFile(string generated, string nameFile)
        {
            FileName = Path.Combine(this.RepertoireDest, nameFile + ".cs");
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
            using (TextWriter writer = new StreamWriter(FileName))
            {
                writer.Write(generated);
            }
        }

        private string m_IndentIncrement = new string(' ', 4);
        private void Indent()
        {
            m_Indent = m_Indent + m_IndentIncrement;
        }

        private void AppendLine()
        {
            AppendLine(String.Empty);
        }

        private void AppendLine(string line)
        {
            AppendIndent();
            m_sb.AppendLine(line);
        }

        private void AppendLine(string fmt, params object[] args)
        {
            string line = String.Format(fmt, args);
            AppendLine(line);
        }

        private void AppendIndent()
        {
            m_sb.Append(m_Indent);
        }

        private void UnIndent()
        {
            if (m_Indent.Length > 0)
            {
                m_Indent = m_Indent.Substring(0, m_Indent.Length - m_IndentIncrement.Length);
            }
        }

        #endregion
    }
}
