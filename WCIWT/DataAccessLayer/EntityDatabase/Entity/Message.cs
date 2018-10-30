using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCIWT.Infrastructure;

namespace EntityDatabase
{
    public class Message : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? UserSenderId { get; set; }
        public virtual User UserSender { get; set; }

        public Guid? UserReceiverId { get; set; }
        public virtual User UserReceiver { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Time { get; set; }

        public bool Seen { get; set; }
        public string Text { get; set; }

        [NotMapped]
        public string TableName => nameof(WCIWTDbContext.Messages);
    }
}
