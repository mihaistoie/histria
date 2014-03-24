using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Changes
{
    public class ObjectDelta
    {
        public ObjectLifetimeEvent Lifetime { get; set; }
        public Guid Target { get; set; }
        public string Class { get; set; }

        public ChangedProperties Properties 
        {
            get; set; 
        }

        public static ObjectDelta InitFromObject(IInterceptedObject target)
        {
            ObjectDelta result = new ObjectDelta()
            {
                Target = target.Uuid,
                Class = target.ClassInfo.CurrentType.FullName,
                Properties = new ChangedProperties(),
            };

            foreach(var prop in target.ClassInfo.Properties)
            {
                result.Properties.Add(new PropertyChange()
                    {
                        PropertyName = prop.Name,
                        Value = target.ClassInfo.CurrentType.GetProperty(prop.Name).GetValue(target, null),
                    });
            }
            return result;
        }

    }
}
