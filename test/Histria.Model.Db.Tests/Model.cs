using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.DbModel.Tests
{
    [NoModel]
    public class BaseModel : IClassModel
    {
        public virtual Guid Uuid { get; set; }
    }


    public class Fruit : BaseModel, IPersistentObj
    {
    }

    [PrimaryKey("CodeISO")]
    public class Country : BaseModel, IPersistentObj
    {
        public virtual string CodeISO { get; set; }
    }

    [PrimaryKey("Red,Green,Blue")]
    public class Color : BaseModel, IPersistentObj
    {
        public virtual int Red  { get; set; }
        public virtual int Green { get; set; }
        public virtual int Blue { get; set; }
    }


}
