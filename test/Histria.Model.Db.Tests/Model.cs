using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.DbModel.Tests
{
    public class BaseModel : IClassModel
    {
        public virtual Guid Uuid { get; set; }
    }


    public class Fruit : BaseModel, IPersistentObj
    {
    }
}
