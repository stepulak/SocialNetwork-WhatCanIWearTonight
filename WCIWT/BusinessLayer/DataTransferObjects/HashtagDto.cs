using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class HashtagDto : DtoBase
    {
        public Guid PostId { get; set; }
        public string Tag { get; set; }
    }
}
