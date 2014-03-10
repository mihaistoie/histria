using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Tests.Rules.Customers
{
    [PrimaryKey("Id")]
    public class HumanBody : InterceptedObject
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        [Association(Relation.Composition, Inv = "Body", Min = 1)]
        public virtual HasOne<Nose> Nose { get; set; }
    }

    [PrimaryKey("Id")]
    public class Nose : InterceptedObject
    {
        public virtual string Id { get; set; }
        public virtual string BodyId { get; set; }
        /* Belongs to Body */
        [Association(Relation.Composition, Inv = "Nose", ForeignKey = "BodyId")]
        public virtual BelongsTo<HumanBody> Body { get; set; }
    }
    
    [RulesFor(typeof(HumanBody))]
    public class HumanBodyRules : IPluginModel
    {
       [Rule(Rule.Correction, Property = "Name")]
       public static void NameIsUpper(HumanBody body, ref object name) 
       {
           if (name == null) return;
           string value = (string)name;
           name = value.ToUpper();
       }

    }


}
