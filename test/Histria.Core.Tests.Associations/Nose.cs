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
    public class Nose : InterceptedObject
    {
        public string CurrentBodyId = null;
        public virtual string Id { get; set; }
        public virtual string BodyId { get; set; }
        /* Belongs to Body */
        [Association(Relation.Composition, Inv = "Nose", ForeignKey = "BodyId")]
        public virtual BelongsTo<HumanBody> Body { get; set; }

        [Rule(Rule.Propagation, Property = "Body")]
        internal void AfterNoseChange()
        {
            CurrentBodyId = (Body.Value == null) ? null : Body.Value.Id;
        }
    }

    
}