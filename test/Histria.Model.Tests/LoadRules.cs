using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model.Tests.ModelToTest
{
    [PrimaryKey("Id")]
    public class HumanBody : BaseModel
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
    }


    [RulesFor(typeof(HumanBody))]
    public class HumanBodyRules : IPluginModel
    {
        [Rule(Rule.Correction, Property = "Name")]
        static public void  NameIsUpper(HumanBody body, ref object name)
        {
            if (name == null) return;
            string value = (string)name;
            name = value.ToUpper();
        }

    }
}
