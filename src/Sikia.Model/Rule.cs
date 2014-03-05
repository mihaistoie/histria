using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    ///<summary>
    /// Type of rules: validation, propagation ...  
    ///</summary>   
    public enum Rule
    {
        Unknown = 0, Validation = 2, Propagation = 4, AfterCreate = 8,
        AfterLoad = 16, BeforeSave = 32, Correction = 64,
        Required = 128
    };
}
