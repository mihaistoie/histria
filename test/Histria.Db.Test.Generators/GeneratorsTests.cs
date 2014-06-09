using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Histria.Db.Generators;
using Histria.Db.Model;
using Histria.Sys;
using System.Text;

namespace Histria.Db.Test.Generators
{
    [TestClass]
    public class GeneratorsTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            ModulePlugIn.Load("Histria.Db.MsSql");
            ModulePlugIn.Initialize(null);
        }

        [TestMethod]
        public void GeneratorsClasses()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/gefsragat?schema=dbo";
            DbSchema ss = DbDrivers.Instance.Schema(DbServices.Url2Protocol(dburl));
            if (ss.DatabaseExists(dburl))
            {
                ss.DropDatabase(dburl);
            }
            ss.CreateDatabase(dburl);


            using (DbSession session = DbDrivers.Instance.Session(dburl))
            {
                using (DbCmd cmd = session.Command(""))
                {
                    var sql = new StringBuilder();
                    sql.AppendLine(" create table exams");
                    sql.AppendLine(" (");
                    sql.AppendLine(" exam_id uniqueidentifier not null,");
                    sql.AppendLine(" exam_key int not null,");
                    sql.AppendLine(" exam_name varchar(50),");
                    sql.AppendLine("PRIMARY KEY(exam_key, exam_id)");
                    sql.AppendLine(" )");
                    cmd.Clear();
                    cmd.Sql = sql.ToString();
                    cmd.Execute();

                    sql.Clear();
                    sql.AppendLine(" create table question_bank");
                    sql.AppendLine(" (");
                    sql.AppendLine(" question_id uniqueidentifier primary key,");
                    sql.AppendLine(" question_exam_id uniqueidentifier,");
                    sql.AppendLine(" question_exam_key int,");
                    sql.AppendLine(" question_text varchar(1024) not null,");
                    sql.AppendLine(" question_point_value decimal");
                    sql.AppendLine(" )");
                    cmd.Clear();
                    cmd.Sql = sql.ToString();
                    cmd.Execute();

                    sql.Clear();
                    sql.AppendLine(" create table anwser_bank");
                    sql.AppendLine(" (");
                    sql.AppendLine("anwser_id           uniqueidentifier primary key,");
                    sql.AppendLine("anwser_question_id  uniqueidentifier,");
                    sql.AppendLine("anwser_text         varchar(1024),");
                    sql.AppendLine("anwser_is_correct   bit");
                    sql.AppendLine(" );");
                    cmd.Clear();
                    cmd.Sql = sql.ToString();
                    cmd.Execute();

                    cmd.Clear();
                    cmd.Sql = "Alter table question_bank Add foreign key (question_exam_key, question_exam_id) references exams(exam_key, exam_id)";
                    cmd.Execute();

                    cmd.Clear();
                    cmd.Sql = "Alter table anwser_bank Add foreign key (anwser_question_id) references question_bank(question_id)";
                    cmd.Execute();

                }
            }
            string dir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Output");
            Generator g = new Generator(dburl, dir, "Histria.Db");
            g.Generate();
            ss.DropDatabase(dburl);

            Assert.IsTrue(File.Exists(dir + @"\Exams.cs"));
            Assert.IsTrue(File.Exists(dir + @"\QuestionBank.cs"));
            Assert.IsTrue(File.Exists(dir + @"\AnwserBank.cs"));
            
        }
    }
}
