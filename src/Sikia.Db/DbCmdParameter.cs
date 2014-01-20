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
        private static int MAX_VARCHAR_SIZE = 128;
        ///<summary>
        /// Name of  parameter
        ///</summary>
        public string Name { get; set; }
        ///<summary>
        /// Maximum size, in bytes, of the data within the column
        ///</summary>
        private int size = 0;
        public int Size {
            get
            {
                if (size <= 0)
                {
                    switch (Type)
                    {
                        case DbType.Varchar :
                            size = MAX_VARCHAR_SIZE;
                            break;
                    }
                }
                return size;
            }
            set
            {
                size = value;
            }
        }
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
            Size = 0;
            Type = DbType.Varchar;
            Value = null;

        }
    }

}
