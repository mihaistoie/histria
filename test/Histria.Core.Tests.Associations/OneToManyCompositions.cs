using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Tests.Associations
{
    [PrimaryKey("Id")]
    public class HBody : InterceptedObject
    {
        /* primary key */
        public virtual string Id { get; set; }
        /* has two hands */
        [Association(Relation.Composition, Inv = "Body", Min = 0, Max = 2)]
        public virtual HasMany<Hand> Hands { get; set; }

        public int HandsCount { get; set; }

        [Rule(Rule.AfterCreate)]
        [Rule(Rule.AfterLoad)]
        public void CalculateHands()
        {
            HandsCount = 0;
        }
        [Rule(Rule.Propagation, Property = "Hands", Operation = RoleOperation.Add)]
        public void AddHand()
        {
            HandsCount = HandsCount + 1;
        }
        [Rule(Rule.Propagation, Property = "Hands", Operation = RoleOperation.Remove)]
        public void RmvHand()
        {
            HandsCount = HandsCount - 1;
        }

    }
    [PrimaryKey("Id")]
    public class Hand : InterceptedObject
    {
        /* primary key */
        public virtual string Id { get; set; }
        /* foreign key */
        public virtual string BodyId { get; set; }
        /* belongs to Body */
        [Association(Relation.Composition, Inv = "Hands", ForeignKey = "BodyId")]
        public virtual BelongsTo<HBody> Body { get; set; }
       
        /* Fingers */
        [Association(Relation.Composition, Inv = "Hand")]
        public virtual HasMany<Finger> Fingers { get; set; }

        public virtual string BodyName { get; set; }
        public virtual string Name { get; set; }
        public virtual string NameToUpper { get; set; }

        [Rule(Rule.Propagation, Property = "Body")]
        public void PropagateBodyId()
        {
            BodyName = Body.Value == null ? null : Body.Value.Id;
        }

        [Rule(Rule.Propagation, Property = "Name")]
        public void PropagateName()
        {
            NameToUpper = Name.ToUpper();
        }
    }

    [PrimaryKey("Id")]
    public class Finger : InterceptedObject
    {
        /* primary key */
        public virtual string Id { get; set; }
        /* foreign key */
        public virtual string HandId { get; set; }
        /* belongs to Body */
        [Association(Relation.Composition, Inv = "Fingers", ForeignKey = "HandId")]
        public virtual BelongsTo<Hand> Hand { get; set; }

    }



}
