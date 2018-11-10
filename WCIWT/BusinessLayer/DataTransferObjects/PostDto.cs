using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class PostDto : DtoBase
    {
        public DateTime Time { get; set; }

        public string Text { get; set; }
        public PostVisibility Visibility { get; set; }
        public Gender GenderRestriction { get; set; }

        public bool HasAgeRestriction { get; set; }
        public int AgeRestrictionFrom { get; set; }
        public int AgeRestrictionTo { get; set; }

        public Guid? UserId { get; set; }
        public virtual UserDto User { get; set; }
    }
}
