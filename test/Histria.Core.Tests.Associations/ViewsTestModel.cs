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
        public virtual string Email { get; set; }
        [Rule(Rule.Correction, Property="FirstName")]
        public void NotNullFirstName(ref string firstName) 
       {
           if (String.IsNullOrEmpty(firstName))
           {
               throw new ApplicationException("First Name can't be empty");
           }
       }

    }

    ///<summary>
    /// A View (Representation) of "User"
    ///</summary>  
    public class UserView : ViewObject<User>
    {
        ///<summary>
        /// Properties exposed from User
        ///</summary>  
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual int Age { get; set; }
        ///<summary>
        /// Owns properties
        ///</summary>  
        public virtual string FullName { get; set; }

        ///<summary>
        /// Owns rules
        ///</summary>  
        [RulePropagation("FirstName")]
        [RulePropagation("LastName")]
        [Rule(Rule.AfterLoad)]
        public void CalculateFullName()
        {
            FullName = FirstName + " " + LastName.ToUpper();
        }
    }
    
    // Not yet necessary
    /////<summary>
    ///// A standalone view
    /////</summary>  
    //public class UserReportSelctions : ViewObject
    //{
    //    [Display("Start First Name")]
    //    public virtual string StartFirstName { get; set; }
    //    [Display("End First Name")]
    //    public virtual string EndLastName { get; set; }
    //}
}
