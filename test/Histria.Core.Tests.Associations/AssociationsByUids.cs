using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Tests.Associations
{
    public class Account: InterceptedObject
    {
        public virtual string Code {get; set;}  
    }

    public  class AccountingEntry : InterceptedObject
    {
        public virtual string AccountCode {get; set;}
       
        [Association(Relation.Association)]
        public virtual HasOne<Account> Account {get; set;} 
        [Rule(Rule.Propagation, Property="Account")]
        internal void UpdateAccountCode() 
        {
            AccountCode = Account.Value == null ? null : Account.Value.Code;
        }
       
    }
}
