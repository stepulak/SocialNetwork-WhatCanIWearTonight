using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class HashtagInPostDto : BaseDto
    {
        public int PostId { get; set; }
        public virtual PostDto Post { get; set; }

        public int HashtagId { get; set; }
        public virtual HashtagDto Hashtag { get; set; }
    }
}
