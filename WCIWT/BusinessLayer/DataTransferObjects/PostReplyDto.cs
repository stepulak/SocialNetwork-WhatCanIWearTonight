using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class PostReplyDto : DtoBase
    {
        [Required]
        public Guid UserId { get; set; }
        public virtual UserDto User { get; set; }

        [Required]
        public Guid PostId { get; set; }
        public virtual PostDto Post { get; set; }

        [Required(ErrorMessage = "Post reply must contain text")]
        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }

        [Required(ErrorMessage="Post reply must contain text")]
        public string Text { get; set; }
    }
}
