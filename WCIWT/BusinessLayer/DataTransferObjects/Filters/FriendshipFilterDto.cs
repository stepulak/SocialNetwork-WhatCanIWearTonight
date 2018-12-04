using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects.Filters
{
    public class FriendshipFilterDto : FilterDtoBase
    {
        public Guid UserA { get; set; }
        public Guid UserB { get; set; }
        public bool IsConfirmed { get; set; }

        public FriendshipFilterDto()
        {
            UserA = Guid.Empty;
            UserB = Guid.Empty;
            IsConfirmed = true;
        }
    }
}
