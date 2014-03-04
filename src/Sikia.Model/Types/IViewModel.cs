using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public interface IViewModel<T> : IClassModel
    {
        T Model { get; set; }  
    }
}
