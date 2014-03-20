using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public class DefaultPropertiesState : PropertiesState
    {
        private Dictionary<string, PropertyState> props = new Dictionary<string, PropertyState>();
        protected override PropertyState GetItemByName(string key) { return props[key]; }
        public override IEnumerator<PropertyState> GetEnumerator()
        {
            return props.Values.GetEnumerator();
        }
        public override void Init(ClassInfoItem ci)
        {
            for (int idx = 0, len = ci.Properties.Count; idx < len; idx++)
            {
                PropInfoItem pi = ci.Properties[idx];
                PropertyState ps = new PropertyState(pi);
                props.Add(pi.Name, ps);
            }
        }
    }
}
