using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base()
        {

        }

        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<HashtagInPost> HashtagInPosts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostReply> PostReplys { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}
