using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects.Filters
{

    public class MessageFilterDto : FilterDtoBase
    {
        public Guid Sender { get; set; }
        public Guid Receiver { get; set; }
        public bool CareAboutRole { get; set; }
        public bool UnseenOnly { get; set; }
        public MessageFilterDto()
        {
            Sender = Guid.Empty;
            Receiver = Guid.Empty;
            CareAboutRole = false;
            UnseenOnly = false;
        }
    }      
}
