using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model.Models
{
    public class BaseModel: IClassModel
    {
        public Guid Uuid { get; set; } 
    }
}
