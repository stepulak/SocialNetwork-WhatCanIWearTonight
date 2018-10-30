using BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataTransferObjects
{
    public class MessageDto : DtoBase
    {
        public Guid? UserSenderId { get; set; }
        public virtual UserDto UserSender { get; set; }

        public Guid? UserReceiverId { get; set; }
        public virtual UserDto UserReceiver { get; set; }
        
        public DateTime Time { get; set; }

        public bool Seen { get; set; }
        public string Text { get; set; }

        public override string ToString() => $"From: {UserSender}, To: {UserReceiver}, Content: {Text}";
    }
}
