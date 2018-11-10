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
        public PostVisibility Visibility { get; set; } = PostVisibility.Public;
        public Gender GenderRestriction { get; set; } = Gender.NoInformation;
        public int UserAge { get; set; } = -1;
        public Guid UserId { get; set; }
    }
}
