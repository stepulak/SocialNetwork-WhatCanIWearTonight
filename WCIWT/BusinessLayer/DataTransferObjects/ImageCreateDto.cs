using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class ImageCreateDto
    {
        public Guid PostId { get; set; }
        public PostDto Post { get; set; }

        public byte[] BinaryImage { get; set; }
    }
}
