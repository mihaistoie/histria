using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Tests.Associations
{
    public class Car : InterceptedObject
    {
        public virtual string Name { get; set; }
        [Association(Relation.Composition, Inv = "Car", Min = 1)]
        public virtual HasOne<SteeringWheel> SteeringWheel { get; set; }
    }


    public class SteeringWheel : InterceptedObject
    {
        public virtual string SerialNumber { get; set; }
        [Association(Relation.Composition, Inv = "SteeringWheel", Min = 1)]
        public virtual BelongsTo<Car> Car { get; set; }
    }

}
