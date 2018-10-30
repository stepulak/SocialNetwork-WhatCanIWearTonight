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
    public class User : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public Gender Gender { get; set; }
        public bool IsAdmin { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Birthdate { get; set; }

        public List<Post> Posts { get; set; }
        public List<PostReply> PostReplys { get; set; }
        public List<Vote> Votes { get; set; }
        public List<Friendship> Friendships { get; set; }
        public List<Message> Messages { get; set; }

        [NotMapped]
        public string TableName => nameof(WCIWTDbContext.Users);
    }
}
