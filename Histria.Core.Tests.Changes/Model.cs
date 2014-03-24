using Histria.Core.Changes;
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

        public List<Tuple<ObjectLifetimeEvent, object[]>> Notifications = new List<Tuple<ObjectLifetimeEvent, object[]>>();

        public ChangeRecorder Recorder = new ChangeRecorder();

        protected override void AOPNotify(ObjectLifetimeEvent objectLifetime, object[] arguments)
        {
            Notifications.Add(new Tuple<ObjectLifetimeEvent,object[]>(objectLifetime, arguments));
            Recorder.Record(this.Uuid, objectLifetime, arguments);
            base.AOPNotify(objectLifetime, arguments);
        }
    }
}
