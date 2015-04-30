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
    //[Index("BodyId")]
    [Index("Body")]
    public class Nose : InterceptedDbObject
    {
        public string CurrentBodyId = null;
        public virtual string Id { get; set; }
        public virtual string BodyId { get; set; }
        /* Belongs to Body */
        [Association(Relation.Composition, Inv = "Nose", ForeignKey = "BodyId")]
        public virtual BelongsTo<HumanBody> Body { get; set; }

        [RulePropagation("Body")]
        internal void AfterNoseChange()
        {
            CurrentBodyId = (Body.Value == null) ? null : Body.Value.Id;
        }
    }

    
}