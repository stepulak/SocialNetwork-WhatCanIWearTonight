using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class FriendshipDto : DtoBase
    {
        public Guid ApplicantId { get; set; }
        public UserDto Applicant { get; set; }

        public Guid RecipientId { get; set; }
        public UserDto Recipient { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
