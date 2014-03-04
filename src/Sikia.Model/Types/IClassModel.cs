using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia
{
    ///<summary>
    /// The classes that support this interface will be loaded by the model manager
    ///</summary>
    public interface IClassModel
    {
        #region Properties
        Guid Uuid { get; set; }
        #endregion
    }
}
