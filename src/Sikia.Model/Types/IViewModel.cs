using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    ///<summary>
    /// View model of nothing
    ///</summary>   
    public interface IViewModel : IClassModel
    {
    }

    ///<summary>
    /// View model of T
    ///</summary>   
    public interface IViewModel<T> : IViewModel
    {
        T Model { get; set; }  
    }
}
