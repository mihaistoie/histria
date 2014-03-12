﻿using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.PropertiesState.Tests
{
    [PrimaryKey("Id")]
    public class HBody : InterceptedObject
    {
        /* primary key */
        public virtual string Id { get; set; }
        /* has two hands */
        [Association(Relation.Composition, Inv = "Body", Min = 0, Max = 2)]
        public virtual HasMany<Hand> Hands { get; set; }

        [Default(Required = true)]
        public virtual string Name { get; set; }
        
        



        [State(Rule.AfterCreate)]
        [State(Rule.AfterLoad)]
        [State(Rule.Propagation, Property = "Hands", Operation = RoleOperation.Remove)]
        [State(Rule.Propagation, Property = "Hands", Operation = RoleOperation.Add)]
        public void Idstate() 
        {
            Properties["Name"].IsDisabled = (Hands.Count > 0);  
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

        public virtual string Name { get; set; }

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
