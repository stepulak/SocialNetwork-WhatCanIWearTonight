using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Time { get; set; }

        public string Text { get; set; }
        public PostVisibility Visibility { get; set; }
        public Gender GenderRestriction { get; set; }

        public bool HasAgeRestriction { get; set; }
        public int AgeRestrictionFrom { get; set; }
        public int AgeRestrictionTo { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public List<PostReply> Replys { get; set; }
        public List<Image> Images { get; set; }
        public List<HashtagInPost> HashtagInPosts { get; set; }
    }
}
