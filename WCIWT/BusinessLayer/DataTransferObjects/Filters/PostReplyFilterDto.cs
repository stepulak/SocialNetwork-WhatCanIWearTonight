using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects.Filters
{
    public class PostReplyFilterDto : FilterDtoBase
    {
        public Guid PostId { get; set; }

        public PostReplyFilterDto()
        {
            PostId = Guid.Empty;
        }
    }
}
