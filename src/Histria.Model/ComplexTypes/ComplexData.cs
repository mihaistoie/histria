using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public abstract class ComplexData: IComplexData
    {
        public static ComplexData ComplexDataFactory(PropInfoItem propInfo, Type declaredType)
        {
            return (ComplexData) Activator.CreateInstance(declaredType);
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
