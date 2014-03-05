using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Changes
{
    public class ChangeRecorder
    {
        public void Record(Guid targetId, ObjectLifetime lifetime, params object[] arguments)
        {
            var rec = new ChangeRecord()
            {
                Id = this.NextId++,
                TargetId = targetId,
                Lifetime = lifetime,
                Arguments = arguments
            };
            this.Records.Add(rec);
        }

        public IList<ChangeRecord> GetRecordsFrom(uint id)
        {
            return this.records.FindAll(r => r.Id >= id);
        }

        public void Clear()
        {
            this.records.Clear();
        }

        public void RemoveBefore(uint id)
        {
            this.records.RemoveAll(r => r.Id < id);
        }

        private List<ChangeRecord> records = new List<ChangeRecord>();
        public IList<ChangeRecord> Records { get { return this.records; } }

        public uint NextId { get; private set; }
    }
}
