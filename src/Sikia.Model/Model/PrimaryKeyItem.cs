﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class PrimaryKeyItem
    {
        private readonly KeysCollection items = new KeysCollection();
        public KeysCollection Items { get { return items; } }
    }
}
