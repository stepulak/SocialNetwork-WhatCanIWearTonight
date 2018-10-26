using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class ImageDto : BaseDto
    {
        public int PostId { get; set; }
        public virtual PostDto Post { get; set; }

        public byte[] BinaryImage { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
    }
}
