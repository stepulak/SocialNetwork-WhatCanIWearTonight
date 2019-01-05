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
        public bool IncludePrivatePosts { get; set; } = false;
        public Gender GenderRestriction { get; set; } = Gender.NoInformation;
        public int UserAge { get; set; } = -1;
        public Guid UserId { get; set; } = Guid.Empty;
        public Guid LoggedUserId { get; set; } = Guid.Empty;
        public List<Guid> PostIdsWithHashtag { get; set; } = null;
        public List<Guid> PostUserIds { get; set; } = null;
    }
}
