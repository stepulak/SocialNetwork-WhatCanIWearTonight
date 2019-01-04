using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Web;

namespace EntityDatabase.Migrations
{
    internal sealed class Configuration: DbMigrationsConfiguration<WCIWTDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WCIWTDbContext context)
        {
            var admin = new User
            {
                // Password: qwerty123
                Id = Guid.Parse("aa00dc64-5c07-40fe-a916-175165b9b90f"),
                Username = "admin",
                Email = "admin@admin.com",
                PasswordHash = "FuQPWbHATtEPh0CO1i6tUqI65/k=",
                PasswordSalt = "WMYJiF/FT8bEchjALl3bCg==",
                Gender = Gender.Male,
                IsAdmin = true,
                Birthdate = new DateTime(1995, 8, 20)
            };
            
            // Password: qwerty123
            var user1 = new User
            {
                Id = Guid.Parse("22d1461d-41db-4a5a-8996-dd0fcf7f5f04"),
                Username = "yohjiYanamoto",
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

            var post8 = new Post
            {
                Id = Guid.Parse("142330f6-3648-46e3-8ece-64ae166dd917"),
                Time = new DateTime(2018, 3, 20, 20, 20, 35),
                Text = "This is a private post from Marie",
                Visibility = PostVisibility.FriendsOnly,
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
                post7,
                post8
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
            
            var user1Friendships = new List<Friendship>
            {
                user1Touser3Friendship
            };
            var user3Friendships = new List<Friendship>
            {
                user1Touser3Friendship
            };
            
            user1.Friendships = user1Friendships;
            //user3.Friendships = user3Friendships;

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
            
            string imageFolder = "~\\Content\\Images\\TestingImages\\";
            var imageGirl1 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "lucia1.jpg"));
            var imageGirl2 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "lucia2.jpg"));
            var imageGirl3 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "lucia3.jpg"));
            var imageGirl4 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "lucia4.jpg"));
            var imageGirl5 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "lucia5.jpg"));
            var imageHelicopter1 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "apache1.jpg"));
            var imageHelicopter2 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "apache2.jpg"));

            var vote1Image1 = new Vote
            {
                Id = Guid.Parse("5f107264-fee7-4a35-ab7f-f5e302c42e20"),
                Type = VoteType.Like,
                User = user1,
                UserId = user1.Id,
            };

            var vote2Image1 = new Vote
            {
                Id = Guid.Parse("5f107264-fee7-4a35-ab7f-f5e302c42e21"),
                Type = VoteType.Dislike,
                User = user2,
                UserId = user2.Id,
            };

            var vote1Image4 = new Vote
            {
                Id = Guid.Parse("5f107264-fee7-4a35-ab7f-f5e302c42e22"),
                Type = VoteType.Like,
                User = user3,
                UserId = user3.Id,
            };

            var vote2Image4 = new Vote
            {
                Id = Guid.Parse("5f107264-fee7-4a35-ab7f-f5e302c42e23"),
                Type = VoteType.Like,
                User = user2,
                UserId = user2.Id,
            };

            var vote3Image4 = new Vote
            {
                Id = Guid.Parse("5f107264-fee7-4a35-ab7f-f5e302c42e24"),
                Type = VoteType.Like,
                User = user1,
                UserId = user1.Id,
            };

            var vote1Image5 = new Vote
            {
                Id = Guid.Parse("5f107264-fee7-4a35-ab7f-f5e302c42e26"),
                Type = VoteType.Like,
                User = user1,
                UserId = user1.Id,
            };

            var image1Post1 = new Image
            {
                Id = Guid.Parse("bbe4fcc1-03f5-433a-b677-0022a7a3b3d1"),
                DislikesCount = 1,
                LikesCount = 1,
                BinaryImage = imageGirl1,
                Post = post1,
                PostId = post1.Id,
                Votes = new List<Vote> { vote1Image1, vote2Image1 },
            };

            var image2Post1 = new Image
            {
                Id = Guid.Parse("bbe4fcc1-03f5-433a-b677-0022a7a3b3d2"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = imageGirl2,
                Post = post1,
                PostId = post1.Id,
                Votes = new List<Vote> { },
            };

            var image3Post1 = new Image
            {
                Id = Guid.Parse("bbe4fcc1-03f5-433a-b677-0022a7a3b3d3"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = imageGirl3,
                Post = post1,
                PostId = post1.Id,
                Votes = new List<Vote> { },
            };

            var image4Post1 = new Image
            {
                Id = Guid.Parse("bbe4fcc1-03f5-433a-b677-0022a7a3b3d4"),
                DislikesCount = 0,
                LikesCount = 3,
                BinaryImage = imageGirl4,
                Post = post1,
                PostId = post1.Id,
                Votes = new List<Vote> { vote1Image4, vote2Image4, vote3Image4 },
            };

            var image5Post1 = new Image
            {
                Id = Guid.Parse("bbe4fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 1,
                BinaryImage = imageGirl5,
                Post = post1,
                PostId = post1.Id,
                Votes = new List<Vote> { vote1Image5 },
            };

            vote1Image1.Image = image1Post1;
            vote1Image1.ImageId = image1Post1.Id;
            vote2Image1.Image = image1Post1;
            vote2Image1.ImageId = image1Post1.Id;
            vote1Image4.Image = image4Post1;
            vote1Image4.ImageId = image4Post1.Id;
            vote2Image4.Image = image4Post1;
            vote2Image4.ImageId = image4Post1.Id;
            vote3Image4.Image = image4Post1;
            vote3Image4.ImageId = image4Post1.Id;
            vote1Image5.Image = image5Post1;
            vote1Image5.ImageId = image5Post1.Id;

            post1.Images = new List<Image> { image1Post1, image2Post1, image3Post1, image4Post1, image5Post1 };

            user1.Messages = user1ToUser3Messages;
            user3.Messages = user1ToUser3Messages;

            /*context.Votes.AddOrUpdate(vote => vote.Id, vote1Image1, vote2Image1, vote1Image4, vote1Image5, vote2Image4, vote3Image4);
            context.Images.AddOrUpdate(image => image.Id, image1Post1, image2Post1, image3Post1, image4Post1, image5Post1);
            context.Users.AddOrUpdate(user => user.Id, admin, user1, user2, user3);
            context.Posts.AddOrUpdate(post => post.Id, post1, post2, post3, post4, post5, post6, post7, post8);
            context.Friendships.AddOrUpdate(friendship => friendship.Id, user1Touser3Friendship);
            context.PostReplys.AddOrUpdate(postReply => postReply.Id, postReply1, postReply2);
            context.Messages.AddOrUpdate(message => message.Id, message1, message2, message3, message4);

            context.SaveChanges();*/
        }
    }
}