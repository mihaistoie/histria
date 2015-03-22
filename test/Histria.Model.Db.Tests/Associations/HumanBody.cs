using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Histria.Model;
using Histria.Core;

namespace Histria.DbModel.Tests
{
    [PrimaryKey("Id")]
    public class HumanBody : InterceptedDbObject
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



