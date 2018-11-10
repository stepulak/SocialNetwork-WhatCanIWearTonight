using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects.Filters
{
    public class VoteFilterDto : FilterDtoBase
    {
        public Guid UserId { get; set; }
        public Guid ImageId { get; set; }
    }
}
