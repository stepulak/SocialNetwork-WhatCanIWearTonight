using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class PostReplyDto : BaseDto
    {
        public int UserId { get; set; }
        public virtual UserDto User { get; set; }

        public int PostId { get; set; }
        public virtual PostDto Post { get; set; }
        
        public DateTime Time { get; set; }

        public string Text { get; set; }
    }
}
