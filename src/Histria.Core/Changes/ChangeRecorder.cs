using Histria.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Histria.Core.Changes
{
    public class ChangeRecorder: KeyedCollection<Guid, ObjectDelta>
    {
        private readonly Dictionary<ObjectLifetimeEvent, Action<Guid, ObjectLifetimeEvent, object[]>> recordActions;

        public ChangeRecorder()
        {
            this.recordActions = new Dictionary<ObjectLifetimeEvent,Action<Guid,ObjectLifetimeEvent,object[]>>()
            {
                {ObjectLifetimeEvent.Created, ObjectCreated},
                {ObjectLifetimeEvent.Changed, PropertyChanged},
            };
        }

        public void Record(Guid targetId, ObjectLifetimeEvent lifetime, params object[] arguments)
        {
            Action<Guid, ObjectLifetimeEvent, object[]> recordAction;
            if(this.recordActions.TryGetValue(lifetime, out recordAction))
            {
                recordAction(targetId, lifetime, arguments);
            }
        }

        private void ObjectCreated(Guid targetId, ObjectLifetimeEvent lifetime, object[] arguments)
        {
            if(this.Contains(targetId))
            {
                throw new InvalidOperationException(String.Format("Object {0} allready created", targetId));
            }
            var target = arguments[0] as IInterceptedObject;
            ObjectDelta delta = ObjectDelta.InitFromObject(target);
            this.Add(delta);
        }
            
        private void PropertyChanged(Guid targetId, ObjectLifetimeEvent lifetime, object[] arguments)
        {
            if(!this.Contains(targetId))
            {
                throw new InvalidOperationException(String.Format("Object {0} not found", targetId));
            }
            ObjectDelta delta = this[targetId];
            string propertyName = (string)arguments[0];
            object oldValue = arguments[1];
            object newValue = arguments[2];
            delta.Properties[propertyName].Value = newValue;
        }

        protected override Guid GetKeyForItem(ObjectDelta item)
        {
            return item.Target;
        }
    }
}
