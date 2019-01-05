using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class PostCreateDto
    {
        [Required(ErrorMessage = "Post must contain some text")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Post visibility must be selected")]
        public PostVisibility Visibility { get; set; }
        [Required(ErrorMessage = "Gender restriction must be selected")]
        public Gender GenderRestriction { get; set; }

        public bool HasAgeRestriction { get; set; }
        public int? AgeRestrictionFrom { get; set; }
        public int? AgeRestrictionTo { get; set; }
    }
}
