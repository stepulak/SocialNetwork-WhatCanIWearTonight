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
    public class Post : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Time { get; set; }

        public string Text { get; set; }
        public PostVisibility Visibility { get; set; }
        public Gender GenderRestriction { get; set; }

        public bool HasAgeRestriction { get; set; }
        public int AgeRestrictionFrom { get; set; }
        public int AgeRestrictionTo { get; set; }

        public Guid? UserId { get; set; }
        public virtual User User { get; set; }

        public List<PostReply> Replys { get; set; }
        public List<Image> Images { get; set; }
        public List<HashtagInPost> HashtagsInPosts { get; set; }

        [NotMapped]
        public string TableName => nameof(WCIWTDbContext.Posts);
    }
}
