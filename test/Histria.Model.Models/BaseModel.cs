using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model.Models
{
    public class BaseModel: IClassModel
    {
        public Guid Uuid { get; set; } 
    }
}
