using Sikia.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sikia.Model;

namespace Sikia.Core.Tests.Associations
{
  public class Nose: InterceptedObject
  {
      public virtual string BodyId { get; set; }
       /* Belongs to Body */
      [Association(Relation.Composition, Inv = "Nose", ForeignKey = "BodyId")]
      public virtual BelongsTo<HumanBody> Body { get; set; }
  }
}