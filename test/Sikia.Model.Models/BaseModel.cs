using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model.Models
{
    public class BaseModel: IModelClass
    {
        public Guid Uuid { get; set; } 
    }
}
