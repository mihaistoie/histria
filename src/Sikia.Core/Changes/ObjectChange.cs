﻿using Sikia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core.Changes
{
    public class ObjectChange
    {
        public ObjectLifetime Lifetime { get; set; }
        public Guid Target { get; set; }
        public string Class { get; set; }
        public List<PropertyChange> Properties { get; set; }
    }
}
