using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityDatabase.Config;

namespace EntityDatabase
{
    public class WCIWTDbContext : DbContext
    {
        public WCIWTDbContext() : base(EntityFrameworkInstaller.ConnectionString)
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WCIWTDbContext, Migrations.Configuration>());

        }

        public WCIWTDbContext(DbConnection connection) : base(connection, true)
        {
            Database.CreateIfNotExists();
        }

        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostReply> PostReplys { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}
