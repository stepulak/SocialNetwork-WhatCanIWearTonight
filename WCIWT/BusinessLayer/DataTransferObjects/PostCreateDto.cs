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
        [Required]
        public string Text { get; set; }
        [Required]
        public PostVisibility Visibility { get; set; }
        [Required]
        public Gender GenderRestriction { get; set; }
        [Required]
        public bool HasAgeRestriction { get; set; }
        public int AgeRestrictionFrom { get; set; }
        public int AgeRestrictionTo { get; set; }
    }
}
