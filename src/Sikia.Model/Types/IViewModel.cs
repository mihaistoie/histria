using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    ///<summary>
    /// View model
    ///</summary>   
    public interface IViewModel<T> : IClassModel
    {
        T Model { get; set; }  
    }
}
