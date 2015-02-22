using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public abstract class CompositeData: IComplexData
    {
        public static CompositeData ComplexDataFactory(PropInfoItem propInfo, Type declaredType)
        {
            return (CompositeData) Activator.CreateInstance(declaredType);
        }
        ///<summary>
        /// Property info 
        ///</summary> 
        public PropInfoItem PropInfo { get; set; }

        ///<summary>
        /// The instance that contains this object
        ///</summary> 
        public IInterceptedObject Instance { get; set; }
    }
}
