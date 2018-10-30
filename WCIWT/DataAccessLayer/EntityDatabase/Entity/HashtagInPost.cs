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
    public class HashtagInPost : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
        
        public Guid HashtagId { get; set; }
        public virtual Hashtag Hashtag { get; set; }

        [NotMapped]
        public string TableName => nameof(WCIWTDbContext.HashtagInPosts);
    }
}
