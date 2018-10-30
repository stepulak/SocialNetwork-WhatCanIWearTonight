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
    public class Vote : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid ImageId { get; set; }
        public virtual Image Image { get; set; }

        public VoteType Type { get; set; }

        [NotMapped]
        public string TableName => nameof(WCIWTDbContext.Votes);
    }
}
