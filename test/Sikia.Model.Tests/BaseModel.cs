using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model.Tests
{
    public class BaseModel: IClassModel
    {
        public Guid Uuid { get; set; } 
    }

    public class BaseView : BaseModel, IViewModel
    {
    }

    public class BaseView<T> : BaseModel, IViewModel<T>
    {
        public T Model { get; set; }
    }

}
