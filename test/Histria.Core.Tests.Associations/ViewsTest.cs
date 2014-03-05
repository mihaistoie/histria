using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Tests.Associations
{
    using Histria.Model;
    ///<summary>
    /// Model (Entity) "User"
    ///</summary>  
    public class User : InterceptedObject
    {
        [Display("First Name")]
        public virtual string FirstName { get; set; }
        [Display("Last Name")]
        public virtual string LastName { get; set; }
        [Display("Age")]
        public virtual int Age { get; set; }
        [Display("Email Address")]
        public virtual int Email { get; set; }
    }

    ///<summary>
    /// A View (Representation) of "User"
    ///</summary>  
    public class UserViewList : ViewObject<User>
    {
        ///<summary>
        /// Properties exposed from User
        ///</summary>  
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        ///<summary>
        /// Owns properties
        ///</summary>  
        public virtual string FullName { get; set; }

        ///<summary>
        /// Owns rules
        ///</summary>  
        [Rule(Rule.Propagation, Property = "FirstName")]
        [Rule(Rule.Propagation, Property = "LastName")]
        [Rule(Rule.AfterLoad)]
        public void CalculateFullName()
        {
            FullName = FirstName + " " + LastName.ToUpper();
        }
    }

    ///<summary>
    /// A standalone view
    ///</summary>  
    public class UserReportSelctions : ViewObject
    {
        [Display("Start First Name")]
        public virtual string StartFirstName { get; set; }
        [Display("End First Name")]
        public virtual string EndLastName { get; set; }
    }
}
