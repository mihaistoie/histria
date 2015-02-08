using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public class Memo : ComplexData
    {
        private string _value;
       
        public string Value {
            get { return _value; }
            set { 
                if (value != _value)
                {
                    // Manually  call object change
                    Instance.AOPChangeProperty(PropInfo, "Value", delegate () {
                        _value = value;
                    });
                }
            }
        }
    }
}
