using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class ViewInfoItem: ClassInfoItem
    {
        public ViewInfoItem(Type cType)
            : base(cType, false)
        {
        }
    }
}
