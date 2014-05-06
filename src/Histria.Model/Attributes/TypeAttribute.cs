using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public class TypeAttribute : System.Attribute
    {
        private string _template;
        public string Template
        {
            get { return _template; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    TypeAttribute tt = TemplateManager.Template(value);
                    if (tt != null)
                    {
                        this.InitFromTemplate(tt);
                    }
                }
                _template = value;

            }
        }

        internal virtual void InitFromTemplate(TypeAttribute template)
        {

        }
        internal virtual bool TryValidate(object value, out string errors)
        {
            errors = null;
            return true;
        }
        internal virtual object SchemaValidation(object value)
        {
            return value;
        }
    }
}


