using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Sikia.Core
{
    public class TranContext : IQueryProvider
    {
        ///<summary>
        /// Id of transaction context 
        ///</summary>
        private Guid uuid = Guid.NewGuid();
        ///<summary>
        /// List of loaded objects 
        ///</summary>
        private InstancesByClass cache = new InstancesByClass();
        ///<summary>
        /// List of lmodified/deleted objects
        ///</summary>
        private InstancesByClass modified = new InstancesByClass();

        #region IQueryProvider
        
        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            throw new NotImplementedException();
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }

        object IQueryProvider.Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
