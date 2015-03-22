using Histria.Core;
using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.DbModel.Tests
{
    public class Account : InterceptedDbObject
    {
        public virtual string Code {get; set;}  
    }

    public class AccountingEntry : InterceptedDbObject
    {
        public virtual string AccountCode {get; set;}
       
        [Association(Relation.Association)]
        public virtual HasOne<Account> Account {get; set;} 
        [RulePropagation("Account")]
        internal void UpdateAccountCode() 
        {
            AccountCode = Account.Value == null ? null : Account.Value.Code;
        }
       
    }
}
