using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Db
{
    ///<summary>
    /// Represents the list of parameters to a DbQuery
    ///</summary>  
    public class DbCmdParameters : IEnumerable<DbCmdParameter>
    {
        
        private List<DbCmdParameter> parameters = new List<DbCmdParameter>();
        public DbCmdParameter Add(string name)
        {
            var p =  new DbCmdParameter();
            p.Name = name;
            parameters.Add(p);
            return p;


        }
        public void Clear()
        {
            parameters.Clear();
        }

        public DbCmdParameter Add(string name, DataTypes type)
        {
            var p = new DbCmdParameter();
            p.Name = name;
            p.Type = type;
            parameters.Add(p);
            return p;
        }
        public DbCmdParameter Add(string name, DataTypes type, int size)
        {
            var p = new DbCmdParameter();
            p.Name = name;
            p.Type = type;
            p.Size = size;
            parameters.Add(p);
            return p;
        }
        public void AddWithValue(string name, object value)
        {
            var p = Add(name);
            p.Value = value;
        }
        
        public void AddWithValue(string name, DataTypes type, object value)
        {
            var p = Add(name, type);
            p.Value = value;
        }
        public void AddWithValue(string name, DataTypes type, object value, int size)
        {
            var p = Add(name, type, size);
            p.Value = value;
        }


        #region IEnumerable<DbQueryParameter> Members

        public IEnumerator<DbCmdParameter> GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        #endregion


        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

   }
}
