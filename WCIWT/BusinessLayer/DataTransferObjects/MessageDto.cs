using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class MessageDto : DtoBase
    {
        [Required]
        public Guid UserSenderId { get; set; }
        public virtual UserDto UserSender { get; set; }

        [Required]
        public Guid UserReceiverId { get; set; }
        public virtual UserDto UserReceiver { get; set; }
        
        [Required(ErrorMessage = "Message must contain time")]
        public DateTime Time { get; set; }

        [Required]
        public bool Seen { get; set; }
        [Required(ErrorMessage = "Message must contain text.")]
        public string Text { get; set; }

        public override string ToString() => $"From: {UserSender}, To: {UserReceiver}, Content: {Text}";
    }
}
