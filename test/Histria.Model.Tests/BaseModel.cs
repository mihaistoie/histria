using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model.Tests
{
    public class BaseModel: IClassModel
    {
        public virtual Guid Uuid { get; set; } 
    }

    public class BaseView : BaseModel, IViewModel
    {
    }

    public class BaseView<T> : BaseModel, IViewModel<T>
    {
        public T Model { get; set; }
    }

}
