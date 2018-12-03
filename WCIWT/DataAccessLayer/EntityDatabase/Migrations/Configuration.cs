using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace EntityDatabase.Migrations
{
    internal sealed class Configuration: DbMigrationsConfiguration<WCIWTDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WCIWTDbContext context)
        {
            var admin = new User
            {
                // Password: PV226jesuper
                Id = Guid.Parse("aa00dc64-5c07-40fe-a916-175165b9b90f"),
                Username = "Mr.Admin",
                Email = "admin@admin.com",
                PasswordHash = "FuQPWbHATtEPh0CO1i6tUqI65",
                PasswordSalt = "WMYJiF/FT8bEchjALl3bCg==",
                Gender = Gender.Male,
                IsAdmin = true,
                Birthdate = new DateTime(1995, 8, 20)
            };
            
            // Password: qwerty123
            var user1 = new User
            {
                Id = Guid.Parse("22d1461d-41db-4a5a-8996-dd0fcf7f5f04"),
                Username = "Yohji Yanamoto",
                Email = "yohji@gmail.com",
                PasswordHash = "FuQPWbHATtEPh0CO1i6tUqI65/k=",
                PasswordSalt = "WMYJiF/FT8bEchjALl3bCg==",
                Gender = Gender.Male,
                IsAdmin = false,
                Birthdate = new DateTime(1990, 8, 20)
            };
            
            var user2 = new User
            {
                Id = Guid.Parse("abcf8c45-cdcb-4e12-9aeb-b6adef036964"),
                Username = "Lenka123",
                Email = "lenka@gmail.com",
                PasswordHash = "FuQPWbHATtEPh0CO1i6tUqI65/k=",
                PasswordSalt = "WMYJiF/FT8bEchjALl3bCg==",
                Gender = Gender.Female,
                IsAdmin = false,
                Birthdate = new DateTime(1950, 8, 20)
            };
            
            var user3 = new User
            {
                Id = Guid.Parse("b8f4bf3c-0367-4ae9-a408-4bd98541f100"),
                Username = "Marie999",
                Email = "marie@gmail.com",
                PasswordHash = "FuQPWbHATtEPh0CO1i6tUqI65/k=",
                PasswordSalt = "WMYJiF/FT8bEchjALl3bCg==",
                Gender = Gender.Female,
                IsAdmin = false,
                Birthdate = new DateTime(2004, 8, 20)
            };

            var post1 = new Post
            {
                Id = Guid.Parse("afbfd424-d186-4747-bd9b-8d894c410936"),
                Time = new DateTime(2018, 10, 20, 20, 20, 35),
                Text = "This is a test public post. Vote for the outfit you like the most",
                Visibility = PostVisibility.Public,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = false,
                UserId = user1.Id,
                User = user1
            };

            var post2 = new Post
            {
                Id = Guid.Parse("e710774f-586c-48ca-9487-9c2d5db8070b"),
                Time = new DateTime(2018, 8, 20, 20, 20, 35),
                Text = "This is another test public post. Vote for the outfit you like the most",
                Visibility = PostVisibility.Public,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = false,
                UserId = user1.Id,
                User = user1
            };
            
            var post3 = new Post
            {
                Id = Guid.Parse("5fcad9cc-ba64-4161-969f-1c941c9e2da5"),
                Time = new DateTime(2018, 5, 20, 20, 20, 35),
                Text = $"This is a test private post. Only friends of {user1.Username} should see this.Vote for the outfit you like the most",
                Visibility = PostVisibility.FriendsOnly,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = false,
                UserId = user1.Id,
                User = user1
            };
            
            var post4 = new Post
            {
                Id = Guid.Parse("09dd3724-6df5-4f16-aeb3-2149d06c071d"),
                Time = new DateTime(2018, 1, 20, 20, 20, 35),
                Text = "This is a test of gender restricted post. Only women should see this.Vote for the outfit you like the most",
                Visibility = PostVisibility.Public,
                GenderRestriction = Gender.Female,
                HasAgeRestriction = false,
                UserId = user1.Id,
                User = user1
            };
            
            var post5 = new Post
            {
                Id = Guid.Parse("142330f6-3648-46e3-8ece-64ae166dd914"),
                Time = new DateTime(2018, 3, 20, 20, 20, 35),
                Text = "This is a test of age restricted post. Only users with age between 20 and 40 should see this.Vote for the outfit you like the most",
                Visibility = PostVisibility.Public,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = true,
                AgeRestrictionFrom = 20,
                AgeRestrictionTo = 40,
                UserId = user1.Id,
                User = user1
            };

            var post6 = new Post
            {
                Id = Guid.Parse("142330f6-3648-46e3-8ece-64ae166dd915"),
                Time = new DateTime(2018, 3, 20, 20, 20, 35),
                Text = "This is a test post from Lenka",
                Visibility = PostVisibility.Public,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = false,
                UserId = user2.Id,
                User = user2
            };
        
            var post7 = new Post
            {
                Id = Guid.Parse("142330f6-3648-46e3-8ece-64ae166dd916"),
                Time = new DateTime(2018, 3, 20, 20, 20, 35),
                Text = "This is a test post from Marie",
                Visibility = PostVisibility.Public,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = false,
                UserId = user3.Id,
                User = user3
            };


            user1.Posts = new List<Post>
            {
                post1,
                post2,
                post3,
                post4,
                post5
            };

            user2.Posts = new List<Post>
            {
                post6
            };
            
            user3.Posts = new List<Post>
            {
                post7
            };

            var postReply1 = new PostReply
            {
                Id = Guid.Parse("9d3a47ff-0fa2-415f-9da5-da14ffeab957"),
                UserId = user1.Id,
                User = user1,
                PostId = post1.Id,
                Post = post1,
                Time = new DateTime(2018, 10, 20, 22, 20, 35),
                Text = "Hey, this is me commenting my own post"
            };
            
            var postReply2 = new PostReply
            {
                Id = Guid.Parse("9d3a47ff-0fa2-415f-9da5-da14ffeab958"),
                UserId = user2.Id,
                User = user2,
                PostId = post1.Id,
                Post = post1,
                Time = new DateTime(2018, 10, 20, 22, 25, 35),
                Text = "Yo, nice outfit!"
            };
           
            user1.PostReplys = new List<PostReply>
            {
                postReply1
            };
            user2.PostReplys = new List<PostReply>
            {
                postReply2
            };
            
            post1.Replys = new List<PostReply>
            {
                postReply1,
                postReply2
            };

            var user1Touser3Friendship =
                new Friendship
                {
                    Id = Guid.Parse("9d3a47ff-0fa2-415f-9da5-da14ffeab959"),
                    Applicant = user1,
                    ApplicantId = user1.Id,
                    Recipient = user3,
                    RecipientId = user3.Id,
                    IsConfirmed = true
                };
            
            var user1Frienships = new List<Friendship>
            {
                user1Touser3Friendship
            };
            var user3Frienships = new List<Friendship>
            {
                user1Touser3Friendship
            };
            
            user1.Friendships = user1Frienships;
            //user3.Friendships = user3Frienships;

            var message1 = new Message
            {
                Id = Guid.Parse("b5bf2ab1-b51c-43bc-a018-55f35a88fac9"),
                UserSenderId = user1.Id,
                UserSender = user1,
                UserReceiver = user3,
                UserReceiverId = user3.Id,
                Seen = true,
                Time = new DateTime(2018, 10, 10, 20, 30, 30),
                Text = "Hey, cool website right? Did you know you can also send private messages?"
            };

            var message2 = new Message
            {
                Id = Guid.Parse("b5bf2ab1-b51c-43bc-a018-55f35a88fac1"),
                UserSenderId = user3.Id,
                UserSender = user3,
                UserReceiver = user1,
                UserReceiverId = user1.Id,
                Seen = true,
                Time = new DateTime(2018, 10, 10, 20, 30, 50),
                Text = "Sure, it's really great. How did you find out about it?"
            };

            var message3 = new Message
            {
                Id = Guid.Parse("b5bf2ab1-b51c-43bc-a018-55f35a88fac2"),
                UserSenderId = user1.Id,
                UserSender = user1,
                UserReceiver = user3,
                UserReceiverId = user3.Id,
                Seen = true,
                Time = new DateTime(2018, 10, 10, 20, 31, 30),
                Text = "I read about it in the new issue of Forbes. How about you?"
            };

            var message4 = new Message
            {
                Id = Guid.Parse("b5bf2ab1-b51c-43bc-a018-55f35a88fac3"),
                UserSenderId = user3.Id,
                UserSender = user3,
                UserReceiver = user1,
                UserReceiverId = user1.Id,
                Seen = false,
                Time = new DateTime(2018, 10, 10, 20, 35, 30),
                Text = "Well, my friend told me about it."
            };
            
            var user1ToUser3Messages = new List<Message>
            {
                message1,
                message2,
                message3,
                message4
            };

            user1.Messages = user1ToUser3Messages;
            user3.Messages = user1ToUser3Messages;

            context.Users.AddOrUpdate(user => user.Id, admin, user1, user2, user3);
            context.Posts.AddOrUpdate(post => post.Id, post1, post2, post3, post4, post5, post6, post7);
            context.Friendships.AddOrUpdate(friendship => friendship.Id, user1Touser3Friendship);
            context.PostReplys.AddOrUpdate(postReply => postReply.Id, postReply1, postReply2);
            context.Messages.AddOrUpdate(message => message.Id, message1, message2, message3, message4);

            context.SaveChanges();
        }
    }
}