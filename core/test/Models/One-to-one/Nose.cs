using Sikia.DataTypes;
using Sikia.Framework;
using Sikia.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestModel.Models.One_to_one
{
  public class Nose: InterceptedObject
  {
      public virtual string BodyId { get; set; }
       /* Belongs to Body */
      [Association(Relation.Composition, Inv = "Nose", Link = "BodyId=Body")]
      public virtual BelongsTo<HumanBody> Body { get; set; }
  }
}