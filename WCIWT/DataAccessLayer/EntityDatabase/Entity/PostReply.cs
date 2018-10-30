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
    public class PostReply : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Time { get; set; }

        public string Text { get; set; }

        [NotMapped]
        public string TableName => nameof(WCIWTDbContext.PostReplys);
    }
}
