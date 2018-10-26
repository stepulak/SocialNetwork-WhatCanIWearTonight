using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class FriendshipDto : BaseDto
    {
        public int? ApplicantId { get; set; }
        public virtual UserDto Applicant { get; set; }

        public int? RecipientId { get; set; }
        public virtual UserDto Recipient { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
