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
            AutomaticMigrationsEnabled = true;
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
                Text = "This is a test public post. Vote for the outfit you like the most #ootd",
                Visibility = PostVisibility.Public,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = false,
                UserId = user3.Id,
            };

            var post2 = new Post
            {
                Id = Guid.Parse("e710774f-586c-48ca-9487-9c2d5db8070b"),
                Time = new DateTime(2018, 8, 20, 20, 20, 35),
                Text = "This is another test public post. Vote for the outfit you like the most #ootd",
                Visibility = PostVisibility.Public,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = false,
                UserId = user1.Id,
            };
            
            var post3 = new Post
            {
                Id = Guid.Parse("5fcad9cc-ba64-4161-969f-1c941c9e2da5"),
                Time = new DateTime(2018, 5, 20, 20, 20, 35),
                Text = $"This is a test private post. Only friends of {user1.Username} should see this.Vote for the outfit you like the most #classy",
                Visibility = PostVisibility.FriendsOnly,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = false,
                UserId = user1.Id,
            };
            
            var post4 = new Post
            {
                Id = Guid.Parse("09dd3724-6df5-4f16-aeb3-2149d06c071d"),
                Time = new DateTime(2018, 1, 20, 20, 20, 35),
                Text = "This is a test of gender restricted post. Only women should see this.Vote for the outfit you like the most #ootd",
                Visibility = PostVisibility.Public,
                GenderRestriction = Gender.Female,
                HasAgeRestriction = false,
                UserId = user2.Id,
            };
            
            var post5 = new Post
            {
                Id = Guid.Parse("142330f6-3648-46e3-8ece-64ae166dd914"),
                Time = new DateTime(2018, 3, 20, 20, 20, 35),
                Text = "This is a test of age restricted post. Only users with age between 20 and 40 should see this.Vote for the outfit you like the most #fresh",
                Visibility = PostVisibility.Public,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = true,
                AgeRestrictionFrom = 20,
                AgeRestrictionTo = 40,
                UserId = user2.Id,
            };

            var post6 = new Post
            {
                Id = Guid.Parse("142330f6-3648-46e3-8ece-64ae166dd915"),
                Time = new DateTime(2018, 3, 20, 20, 20, 35),
                Text = "This is a test post from Lenka #ootd",
                Visibility = PostVisibility.Public,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = false,
                UserId = user2.Id,
            };
        
            var post7 = new Post
            {
                Id = Guid.Parse("142330f6-3648-46e3-8ece-64ae166dd916"),
                Time = new DateTime(2018, 3, 20, 20, 20, 35),
                Text = "This is a test post from Marie #ootd",
                Visibility = PostVisibility.Public,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = false,
                UserId = user3.Id,
            };

            var post8 = new Post
            {
                Id = Guid.Parse("142330f6-3648-46e3-8ece-64ae166dd917"),
                Time = new DateTime(2018, 3, 20, 20, 20, 35),
                Text = "This is a private post from Marie #classy",
                Visibility = PostVisibility.FriendsOnly,
                GenderRestriction = Gender.NoInformation,
                HasAgeRestriction = false,
                UserId = user3.Id,
            };


            var postReply1 = new PostReply
            {
                Id = Guid.Parse("9d3a47ff-0fa2-415f-9da5-da14ffeab957"),
                UserId = user1.Id,
                PostId = post1.Id,
                Time = new DateTime(2018, 10, 20, 22, 20, 35),
                Text = "Hey, this is me commenting my own post"
            };
            
            var postReply2 = new PostReply
            {
                Id = Guid.Parse("9d3a47ff-0fa2-415f-9da5-da14ffeab958"),
                UserId = user2.Id,
                PostId = post1.Id,
                Time = new DateTime(2018, 10, 20, 22, 25, 35),
                Text = "Yo, nice outfit!"
            };
           

            var user1Touser3Friendship =
                new Friendship
                {
                    Id = Guid.Parse("9d3a47ff-0fa2-415f-9da5-da14ffeab959"),
                    ApplicantId = user1.Id,
                    RecipientId = user3.Id,
                    IsConfirmed = true
                };
            

            var message1 = new Message
            {
                Id = Guid.Parse("b5bf2ab1-b51c-43bc-a018-55f35a88fac9"),
                UserSenderId = user1.Id,
                UserReceiverId = user3.Id,
                Seen = true,
                Time = new DateTime(2018, 10, 10, 20, 30, 30),
                Text = "Hey, cool website right? Did you know you can also send private messages?"
            };

            var message2 = new Message
            {
                Id = Guid.Parse("b5bf2ab1-b51c-43bc-a018-55f35a88fac1"),
                UserSenderId = user3.Id,
                UserReceiverId = user1.Id,
                Seen = true,
                Time = new DateTime(2018, 10, 10, 20, 30, 50),
                Text = "Sure, it's really great. How did you find out about it?"
            };

            var message3 = new Message
            {
                Id = Guid.Parse("b5bf2ab1-b51c-43bc-a018-55f35a88fac2"),
                UserSenderId = user1.Id,
                UserReceiverId = user3.Id,
                Seen = true,
                Time = new DateTime(2018, 10, 10, 20, 31, 30),
                Text = "I read about it in the new issue of Forbes. How about you?"
            };

            var message4 = new Message
            {
                Id = Guid.Parse("b5bf2ab1-b51c-43bc-a018-55f35a88fac3"),
                UserSenderId = user3.Id,
                UserReceiverId = user1.Id,
                Seen = false,
                Time = new DateTime(2018, 10, 10, 20, 35, 30),
                Text = "Well, my friend told me about it."
            };
            
            
            string imageFolder = "~\\Content\\Images\\TestingImages\\";
            var imageGirl1 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "lucia1.jpg"));
            var imageGirl2 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "lucia2.jpg"));
            var imageGirl3 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "lucia3.jpg"));
            var imageGirl4 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "lucia4.jpg"));
            var imageGirl5 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "lucia5.jpg"));
            var imageHelicopter1 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "apache1.jpg"));
            var imageHelicopter2 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "apache2.jpg"));

            var image1Post2 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0201.jpg"));
            var image2Post2 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0202.jpg"));

            var image1Post3 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0301.jpg"));
            var image2Post3 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0302.jpg"));
            var image3Post3 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0303.jpg"));

            var image1Post4 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0401.jpg"));
            var image2Post4 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0402.jpg"));
            var image3Post4 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0403.jpg"));

            var image1Post5 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0501.jpg"));
            var image2Post5 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0502.jpg"));
            var image3Post5 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0503.jpg"));

            var image1Post6 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0601.jpg"));
            var image2Post6 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0602.jpg"));
            var image3Post6 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0603.jpg"));

            var image1Post7 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0701.jpg"));
            var image2Post7 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0702.jpg"));
            var image3Post7 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0703.jpg"));

            var image1Post8 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0801.jpg"));
            var image2Post8 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0802.jpg"));
            var image3Post8 = File.ReadAllBytes(HttpContext.Current.Server.MapPath(imageFolder + "img0803.jpg"));

            var vote1Image1 = new Vote
            {
                Id = Guid.Parse("5f107264-fee7-4a35-ab7f-f5e302c42e20"),
                Type = VoteType.Like,
                UserId = user1.Id,
            };

            var vote2Image1 = new Vote
            {
                Id = Guid.Parse("5f107264-fee7-4a35-ab7f-f5e302c42e21"),
                Type = VoteType.Dislike,
                UserId = user2.Id,
            };

            var vote1Image4 = new Vote
            {
                Id = Guid.Parse("5f107264-fee7-4a35-ab7f-f5e302c42e22"),
                Type = VoteType.Like,
                UserId = user3.Id,
            };

            var vote2Image4 = new Vote
            {
                Id = Guid.Parse("5f107264-fee7-4a35-ab7f-f5e302c42e23"),
                Type = VoteType.Like,
                UserId = user2.Id,
            };

            var vote3Image4 = new Vote
            {
                Id = Guid.Parse("5f107264-fee7-4a35-ab7f-f5e302c42e24"),
                Type = VoteType.Like,
                UserId = user1.Id,
            };

            var vote1Image5 = new Vote
            {
                Id = Guid.Parse("5f107264-fee7-4a35-ab7f-f5e302c42e26"),
                Type = VoteType.Like,
                UserId = user1.Id,
            };

            var image1Post1 = new Image
            {
                Id = Guid.Parse("bbe4fcc1-03f5-433a-b677-0022a7a3b3d1"),
                DislikesCount = 1,
                LikesCount = 1,
                BinaryImage = imageGirl1,
                PostId = post1.Id,
            };

            var image2Post1 = new Image
            {
                Id = Guid.Parse("bbe4fcc1-03f5-433a-b677-0022a7a3b3d2"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = imageGirl2,
                PostId = post1.Id,
            };

            var image3Post1 = new Image
            {
                Id = Guid.Parse("bbe4fcc1-03f5-433a-b677-0022a7a3b3d3"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = imageGirl3,
                PostId = post1.Id,
            };

            var image4Post1 = new Image
            {
                Id = Guid.Parse("bbe4fcc1-03f5-433a-b677-0022a7a3b3d4"),
                DislikesCount = 0,
                LikesCount = 3,
                BinaryImage = imageGirl4,
                PostId = post1.Id,
            };

            var image5Post1 = new Image
            {
                Id = Guid.Parse("bbe4fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 1,
                BinaryImage = imageGirl5,
                PostId = post1.Id,
            };

            var image1Post2Obj = new Image
            {
                Id = Guid.Parse("abe4fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image1Post2,
                PostId = post2.Id,
            };

            var image2Post2Obj = new Image
            {
                Id = Guid.Parse("abe5fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image2Post2,
                PostId = post2.Id,
            };

            var image1Post3Obj = new Image
            {
                Id = Guid.Parse("abe6fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image1Post3,
                PostId = post3.Id,
            };

            var image2Post3Obj = new Image
            {
                Id = Guid.Parse("abe7fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image2Post3,
                PostId = post3.Id,
            };

            var image3Post3Obj = new Image
            {
                Id = Guid.Parse("abe8fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image3Post3,
                PostId = post3.Id,
            };

            var image1Post4Obj = new Image
            {
                Id = Guid.Parse("ace6fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image1Post4,
                PostId = post4.Id,
            };

            var image2Post4Obj = new Image
            {
                Id = Guid.Parse("ace7fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image2Post4,
                PostId = post4.Id,
            };

            var image3Post4Obj = new Image
            {
                Id = Guid.Parse("ace8fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image3Post4,
                PostId = post4.Id,
            };

            var image1Post5Obj = new Image
            {
                Id = Guid.Parse("ade6fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image1Post5,
                PostId = post5.Id,
            };

            var image2Post5Obj = new Image
            {
                Id = Guid.Parse("ade7fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image2Post5,
                PostId = post5.Id,
            };

            var image3Post5Obj = new Image
            {
                Id = Guid.Parse("ade8fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image3Post5,
                PostId = post5.Id,
            };

            var image1Post6Obj = new Image
            {
                Id = Guid.Parse("aee6fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image1Post6,
                PostId = post6.Id,
            };

            var image2Post6Obj = new Image
            {
                Id = Guid.Parse("aee7fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image2Post6,
                PostId = post6.Id,
            };

            var image3Post6Obj = new Image
            {
                Id = Guid.Parse("aee8fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image3Post6,
                PostId = post6.Id,
            };

            var image1Post7Obj = new Image
            {
                Id = Guid.Parse("cde6fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image1Post7,
                PostId = post7.Id,
            };

            var image2Post7Obj = new Image
            {
                Id = Guid.Parse("cde7fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image2Post7,
                PostId = post7.Id,
            };

            var image3Post7Obj = new Image
            {
                Id = Guid.Parse("cde8fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image3Post7,
                PostId = post7.Id,
            };

            var image1Post8Obj = new Image
            {
                Id = Guid.Parse("cce6fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image1Post8,
                PostId = post8.Id,
            };

            var image2Post8Obj = new Image
            {
                Id = Guid.Parse("cce7fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image2Post8,
                PostId = post8.Id,
            };

            var image3Post8Obj = new Image
            {
                Id = Guid.Parse("cce8fcc1-03f5-433a-b677-0022a7a3b3d5"),
                DislikesCount = 0,
                LikesCount = 0,
                BinaryImage = image3Post8,
                PostId = post8.Id,
            };

            vote1Image1.ImageId = image1Post1.Id;
            vote2Image1.ImageId = image1Post1.Id;
            vote1Image4.ImageId = image4Post1.Id;
            vote2Image4.ImageId = image4Post1.Id;
            vote3Image4.ImageId = image4Post1.Id;
            vote1Image5.ImageId = image5Post1.Id;

            var hashtagOotdPost1 = new Hashtag
            {
                Id = Guid.Parse("3150e617-f069-4fc9-98bf-78e87b4084ad"),
                Tag = "#ootd",
                PostId = post1.Id,
            };
            var hashtagOotdPost2 = new Hashtag
            {
                Id = Guid.Parse("0411449a-4f06-463b-bcbb-9f15843a4ecd"),
                Tag = "#ootd",
                PostId = post2.Id,
            };
            var hashtagClassydPost3 = new Hashtag
            {
                Id = Guid.Parse("fb34c6a6-390e-41e2-aae5-79ed941df01a"),
                Tag = "#classy",
                PostId = post3.Id,
            };
            var hashtagOotdPost4 = new Hashtag
            {
                Id = Guid.Parse("c8b27680-20a0-474c-ae96-ff10815bf6f0"),
                Tag = "#ootd",
                PostId = post4.Id,
            };
            var hashtagFreshdPost5 = new Hashtag
            {
                Id = Guid.Parse("695e5879-3b3c-4cf3-bb9e-5af7ce40d069"),
                Tag = "#fresh",
                PostId = post5.Id,
            };
            var hashtagOotdPost6 = new Hashtag
            {
                Id = Guid.Parse("67c634c7-e8dc-44f6-a304-2db8c83f46eb"),
                Tag = "#ootd",
                PostId = post6.Id,
            };
            var hashtagOotdPost7 = new Hashtag
            {
                Id = Guid.Parse("15595c32-07ba-4fc7-8be5-f9d1a754b34c"),
                Tag = "#ootd",
                PostId = post7.Id,
            };
            var hashtagClassydPost8 = new Hashtag
            {
                Id = Guid.Parse("276fe6f5-28ec-4f5b-8e80-b606ee002e6b"),
                Tag = "#classy",
                PostId = post8.Id,
            };

            context.Votes.AddOrUpdate(vote => vote.Id, vote1Image1, vote2Image1, vote1Image4, vote1Image5, vote2Image4, vote3Image4);
            context.Images.AddOrUpdate(image => image.Id, image1Post1, image2Post1, image3Post1, image4Post1, image5Post1,
                image1Post2Obj, image2Post2Obj, image1Post3Obj, image2Post3Obj, image3Post3Obj,
                image1Post4Obj, image2Post4Obj, image3Post4Obj,
                image1Post5Obj, image2Post5Obj, image3Post5Obj,
                image1Post6Obj, image2Post6Obj, image3Post6Obj,
                image1Post7Obj, image2Post7Obj, image3Post7Obj,
                image1Post8Obj, image2Post8Obj, image3Post8Obj);
            context.Users.AddOrUpdate(user => user.Id, admin, user1, user2, user3);
            context.Posts.AddOrUpdate(post => post.Id, post1, post2, post3, post4, post5, post6, post7, post8);
            context.Friendships.AddOrUpdate(friendship => friendship.Id, user1Touser3Friendship);
            context.PostReplys.AddOrUpdate(postReply => postReply.Id, postReply1, postReply2);
            context.Messages.AddOrUpdate(message => message.Id, message1, message2, message3, message4);
            context.Hashtags.AddOrUpdate(hashtag => hashtag.Id, hashtagOotdPost1, hashtagOotdPost2, hashtagOotdPost4, hashtagOotdPost6,
                hashtagOotdPost7, hashtagFreshdPost5, hashtagClassydPost3, hashtagClassydPost8);

            context.SaveChanges();
        }
    }
}