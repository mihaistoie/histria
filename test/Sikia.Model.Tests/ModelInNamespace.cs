using Sikia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model.Tests.XXX
{
    public class ClassInXXX : BaseModel
    {
    }

    public class Account : BaseModel
    {
        [Display("Account Code")]
        public virtual string Code { get; set; }
        [Display("Caption")]
        public virtual string Title { get; set; }
    }

    public class AccountView : BaseView<Account>, IViewModel<Account>
    {
        public virtual  string Code { get; set; }

        public virtual string AccountTitle
        {
            get { return Model.Title; }
            set { Model.Title = value; }
        }
    }

}
