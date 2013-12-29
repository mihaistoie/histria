using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Framework;

namespace TestRules.Models
{

    [Display("Gender", Description = "Gender of Customer")]
    public enum CustomerGender
    {
        [Display("Masculin")]
        Male,
        [Display("Féminin")]
        Female
    };

}