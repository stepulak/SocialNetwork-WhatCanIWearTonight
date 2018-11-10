using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCIWT.Infrastructure;

namespace EntityDatabase
{
    public class Hashtag : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Tag { get; set; }

        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }

        [NotMapped]
        public string TableName => nameof(WCIWTDbContext.HashtagInPosts);
    }
}
