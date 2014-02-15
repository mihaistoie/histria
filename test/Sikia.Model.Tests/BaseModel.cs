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
}
