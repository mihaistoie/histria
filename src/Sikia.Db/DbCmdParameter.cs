using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db
{
    ///<summary>
    /// Represents a parameter to a DbQuery
    ///</summary>
    public class DbCmdParameter
    {
        ///<summary>
        /// Name of  parameter
        ///</summary>
        public string Name { get; set; }
        ///<summary>
        /// Maximum size, in bytes, of the data within the column
        ///</summary>
        public int Size { get; set; }
        ///<summary>
        /// Value of  parameter
        ///</summary>
        public object Value { get; set; }
        ///<summary>
        /// Type of  parameter
        ///</summary>
        public DbType Type { get; set; }
        public DbCmdParameter()
        {
            Name = "";
            Size = -1;
            Type = DbType.Varchar;
            Value = null;

        }
    }

}
