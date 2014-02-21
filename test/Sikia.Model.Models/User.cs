using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sikia.Model.Models
{
    public class User : BaseModel
    {
        public virtual string Name { get; set; }
    }
}