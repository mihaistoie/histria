using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Histria.Model;

namespace Histria.Model.Models
{
    public class User : BaseModel
    {
        [DtString()]
        public virtual string Name { get; set; }
    }
}