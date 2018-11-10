using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects.Filters
{
    public enum MessageUserFilterType
    {
        Sender,
        Receiver,
        Both
    }

    public class MessageFilterDto : FilterDtoBase
    {
        public Guid UserId { get; set; }
        public MessageUserFilterType UserFilterType { get; set; } = MessageUserFilterType.Both;
    }
}
