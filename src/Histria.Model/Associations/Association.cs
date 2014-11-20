using Histria.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    ///<summary>
    /// Base class for all associations  
    ///</summary> 
    public abstract class Association : IAssociation
    {

 
        ///<summary>
        /// Property info 
        ///</summary> 
        public PropInfoItem PropInfo { get; set; }

        ///<summary>
        /// The instance that contains this association
        ///</summary> 
        public IInterceptedObject Instance { get; set; }


        #region Views
        private List<Association> associationViews;
        public List<Association> AssociationViews
        {
            get
            {
                if (associationViews == null)
                {
                    associationViews = new List<Association>();
                }
                return associationViews;
            }
        }

        #endregion
    }
}
