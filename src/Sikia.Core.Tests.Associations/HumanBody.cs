using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sikia.Core;

namespace UnitTestModel.Models.One_to_one
{
    public class HumanBody : InterceptedObject
    {
        public virtual string Body { get; set; }
        [Association(Relation.Composition, Inv = "Body", Min = 1)]
        public virtual HasOne<Nose> Nose { get; set; }
    }
}



