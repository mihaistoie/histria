using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Histria.Core;
using Histria.Model;

namespace Histria.Core.Tests.Associations
{
    [PrimaryKey("Id")]
    public class HumanBody : InterceptedObject
    {
        public string CurrentNoseId = null; 
        public virtual string Id { get; set; }
        public virtual string Body { get; set; }
        [Association(Relation.Composition, Inv = "Body", Min = 1)]
        public virtual HasOne<Nose> Nose { get; set; }
        [RulePropagation("Nose")]
        internal void AfterNoseChange()
        {
            CurrentNoseId = (Nose.Value == null) ? null : Nose.Value.Id;
        }
    }
}



