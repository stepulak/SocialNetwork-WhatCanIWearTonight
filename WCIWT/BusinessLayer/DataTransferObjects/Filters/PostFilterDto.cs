using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects.Filters
{
    public class PostFilterDto : FilterDtoBase
    {
        public PostVisibility Visibility { get; set; }
        public Gender GenderRestriction { get; set; } 
        public int UserAge { get; set; }
        public Guid UserId { get; set; }

        public PostFilterDto()
        {
            Visibility = PostVisibility.Public;
            GenderRestriction = Gender.NoInformation;
            UserAge = -1;
            UserId = Guid.Empty;
        }
    }
}
