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
        public bool IncludePrivatePosts { get; set; }
        public Gender GenderRestriction { get; set; }
        public int UserAge { get; set; }
        public Guid UserId { get; set; }

        public PostFilterDto()
        {
            IncludePrivatePosts = false;
            GenderRestriction = Gender.NoInformation;
            UserAge = -1;
            UserId = Guid.Empty;
        }
    }
}
