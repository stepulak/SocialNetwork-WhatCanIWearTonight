using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DataTransferObjects
{
    public enum Gender
    {
        [Display(Name = "None")]
        NoInformation,
        [Display(Name = "Male")]
        Male,
        [Display(Name = "Female")]
        Female,
    }
}
