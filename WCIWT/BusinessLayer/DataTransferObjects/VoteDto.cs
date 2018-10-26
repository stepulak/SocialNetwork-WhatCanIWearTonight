using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class VoteDto : BaseDto
    {
        public int UserId { get; set; }
        public virtual UserDto User { get; set; }

        public int ImageId { get; set; }
        public virtual ImageDto Image { get; set; }

        public VoteType Type { get; set; }
    }
}
