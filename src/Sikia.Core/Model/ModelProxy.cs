﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core.Model
{
    public static class ModelProxy
    {
        public static ModelManager Model()
        {
            return ModelManager.Instance;
        }
    }
}
