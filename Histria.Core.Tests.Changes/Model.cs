using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Histria.Core.Tests.Changes
{
    public class Simple: InterceptedObject
    {
        [Default(Required = true)]
        public virtual string Name { get; set; }
        
        public virtual DateTime Date { get; set; }

        public virtual decimal Value { get; set; }
    }
}
