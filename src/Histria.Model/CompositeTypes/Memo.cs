using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public class Memo : CompositeData
    {
        private string _value;
       
        public string Value {
            get { return _value; }
            set { 
                if (value != _value)
                {
                    // Manually  call object change
                    ChangePropertyValue("Value", delegate () { _value = value; });
                }
            }
        }
     
    }
}
