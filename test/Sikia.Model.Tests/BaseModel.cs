using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model.Tests
{
    public class BaseModel: IModelClass
    {
        public Guid Uuid { get; set; } 
    }

    public class BaseView<T> : IModelView<T>
    {
        public Guid Uuid { get; set; }
        public T Model { get; set; }
    }

}
