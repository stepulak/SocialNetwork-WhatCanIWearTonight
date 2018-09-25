using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class Message
    {
        [Key]
        public uint Id { get; set; }

        public uint UserSenderId { get; set; }
        public User UserSender { get; set; }

        public uint UserReceiverId { get; set; }
        public User UserReceiver { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Time { get; set; }

        public bool Seen { get; set; }
        public string Text { get; set; }
    }
}
