using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Tests.Rules.Customers
{

    public class MemoTyped : Memo
    {
        private string _contentType;
        public string ContentType
        {
            get { return _contentType; }
            set
            {
                if (value != _contentType)
                {
                    // Manually  call object change
                    Instance.AOPChangeProperty(PropInfo, "ContentType", delegate()
                    {
                        _contentType = value;
                    });
                }
            }
        }
    }

    public partial class BlogItem : InterceptedObject
    {
        public virtual MemoTyped Text { get; set; }
    }

    public partial class BlogItem : InterceptedObject
    {
        public int CCount;
        public int RuleHit;
        [Rule(Rule.Propagation, Property = "Text.Value")]
        private void Calculate()
        {
            CCount = string.IsNullOrEmpty(Text.Value) ? 0 : Text.Value.Length;
            RuleHit++;
        }
        [Rule(Rule.Propagation, Property = "Text.ContentType")]
        private void Empty()
        {
            Text.Value = null;
            RuleHit++;
        }
        [Rule(Rule.Propagation, Property = "Text")]
        private void IncHit()
        {
            RuleHit++;
        }
        [Rule(Rule.AfterCreate)]
        private void InitAfterCreate()
        {
            NoRules(() => {
                Text.ContentType = "text/html";
            });
        }
    }

}
