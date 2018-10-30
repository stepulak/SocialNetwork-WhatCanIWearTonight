using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class HashtagInPostDto : DtoBase
    {
        public Guid PostId { get; set; }
        public virtual PostDto Post { get; set; }

        public Guid HashtagId { get; set; }
        public virtual HashtagDto Hashtag { get; set; }
    }
}
