using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public interface IModelView<T>: IModelClass
    {
        T Model { get; set; }  
    }
}
