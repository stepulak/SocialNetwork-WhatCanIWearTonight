using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class PostReplyDto : DtoBase
    {
        public Guid UserId { get; set; }
        public virtual UserDto User { get; set; }

        public Guid PostId { get; set; }
        public virtual PostDto Post { get; set; }
        
        public DateTime Time { get; set; }

        public string Text { get; set; }
    }
}
