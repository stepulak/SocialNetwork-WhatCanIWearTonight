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
        [Required(ErrorMessage = "Has age restriction must be selected")]
        public bool HasAgeRestriction { get; set; }
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Age restriction from must be a natural number")]
        public int AgeRestrictionFrom { get; set; }
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Age restriction to must be a natural number")]
        public int AgeRestrictionTo { get; set; }
    }
}
