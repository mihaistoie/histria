using Sikia.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Settings
{
    
    public class ApplicationConfig
    {

        public Type[] Types {get; set;}
        public string[] Namespaces = {"Models"};
        public JsonValue Databases { get; set; }
    }
}
