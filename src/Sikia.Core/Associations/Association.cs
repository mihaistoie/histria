﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core
{
    public abstract class Association<T> : IAssociation where T : InterceptedObject
    {
    }
}
