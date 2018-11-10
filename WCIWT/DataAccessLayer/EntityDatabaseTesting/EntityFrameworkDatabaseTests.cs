using System;
using System.Linq;
using EntityDatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityDatabaseTesting
{
    [TestClass]
    public class EntityFrameworkDatabaseTests
    {
        [TestCleanup]
        public void AfterRun()
        {
            using (var db = new WCIWTDbContext())
            {
                db.Friendships.RemoveRange(db.Friendships);
                db.PostReplys.RemoveRange(db.PostReplys);
                db.Posts.RemoveRange(db.Posts);
                db.Messages.RemoveRange(db.Messages);
                db.Users.RemoveRange(db.Users);
            }
        }

        [TestMethod]
        public void AddSingleUser()
        {
            const string username = "Miss Fortune";
            const string passwd = "25afd84d";
            var birthdate = new DateTime(1999, 12, 12);

            using (var db = new WCIWTDbContext())
            {
                var user = new User
                {
                    Id = ToGuid(1),
                    Username = username,
                    PasswordHash = passwd,
                    Birthdate = birthdate,
                    Gender = Gender.Female,
                    IsAdmin = false
                };
                db.Users.Add(user);
                db.SaveChanges();
            }

            using (var db = new WCIWTDbContext())
            {
                Assert.AreEqual(db.Users.Count(), 1);
                var user = db.Users.First();
                Assert.AreEqual(user.Id, ToGuid(1));
                Assert.AreEqual(user.Username, username);
                Assert.AreEqual(user.PasswordHash, passwd);
                Assert.AreEqual(user.Birthdate, birthdate);
                Assert.AreEqual(user.Gender, Gender.Female);
                Assert.IsFalse(user.IsAdmin);
                Assert.IsNull(user.Messages);
                Assert.IsNull(user.PostReplys);
                Assert.IsNull(user.Posts);
                Assert.IsNull(user.Votes);
                Assert.IsNull(user.Friendships);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void CreateFriendship()
        {
            using (var db = new WCIWTDbContext())
            {
                db.Users.Add(CreateSampleUser(1));
                db.Users.Add(CreateSampleUser(2));
                db.Friendships.Add(new Friendship
                {
                    Id = ToGuid(3),
                    ApplicantId = ToGuid(1),
                    RecipientId = ToGuid(2),
                    IsConfirmed = true
                });
                db.SaveChanges();
            }

            using (var db = new WCIWTDbContext())
            {
                Assert.AreEqual(db.Users.Count(), 2);
                Assert.AreEqual(db.Friendships.Count(), 1);
                var friendship = db.Friendships.First();
                Assert.AreEqual(friendship.ApplicantId, ToGuid(1));
                Assert.AreEqual(friendship.RecipientId, ToGuid(2));
                Assert.IsTrue(friendship.IsConfirmed);
                Assert.IsNotNull(friendship.Applicant);
                Assert.IsNotNull(friendship.Recipient);
                Assert.AreEqual(friendship.Applicant.Id, ToGuid(1));
                Assert.AreEqual(friendship.Recipient.Id, ToGuid(2));
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void AddPostAndReply()
        {
            const string postText = "Do you like my new AGM-114 Hellfire missile?";
            const string replyText = "Hell yeah baby fire'em!";
            var postTime = new DateTime(2018, 10, 19, 11, 12, 13);
            var replyTime = new DateTime(2018, 10, 19, 11, 12, 15);
            
            using (var db = new WCIWTDbContext())
            {
                db.Users.Add(CreateSampleUser(1));
                db.Users.Add(CreateSampleUser(2));
                db.Posts.Add(new Post
                {
                    Id = ToGuid(1),
                    GenderRestriction = Gender.ApacheHelicopter,
                    HasAgeRestriction = true,
                    AgeRestrictionFrom = 18,
                    AgeRestrictionTo = 25,
                    UserId = ToGuid(1),
                    Visibility = PostVisibility.Public,
                    Time = postTime,
                    Text = postText,
                });
                db.PostReplys.Add(new PostReply
                {
                    Id = ToGuid(1),
                    PostId = ToGuid(1),
                    UserId = ToGuid(2),
                    Time = replyTime,
                    Text = replyText
                });
                db.SaveChanges();
            }

            using (var db = new WCIWTDbContext())
            {
                Assert.AreEqual(db.Users.Count(), 2);
                Assert.AreEqual(db.Posts.Count(), 1);
                Assert.AreEqual(db.PostReplys.Count(), 1);

                var post = db.Posts.First();
                Assert.AreEqual(post.Id, ToGuid(1));
                Assert.AreEqual(post.UserId, ToGuid(1));
                Assert.IsNotNull(post.User);
                Assert.AreEqual(post.User.Id, ToGuid(1));
                Assert.IsTrue(post.HasAgeRestriction);
                Assert.AreEqual(post.AgeRestrictionFrom, 18);
                Assert.AreEqual(post.AgeRestrictionTo, 25);
                Assert.AreEqual(post.GenderRestriction, Gender.ApacheHelicopter);
                Assert.AreEqual(post.Visibility, PostVisibility.Public);
                Assert.AreEqual(post.Time, postTime);
                Assert.AreEqual(post.Text, postText);

                var reply = db.PostReplys.First();
                Assert.AreEqual(reply.Id, ToGuid(1));
                Assert.AreEqual(reply.UserId, ToGuid(2));
                Assert.IsNotNull(reply.User);
                Assert.AreEqual(reply.PostId, ToGuid(1));
                Assert.IsNotNull(reply.Post);
                Assert.AreEqual(reply.Post.Id, ToGuid(1));
                Assert.AreEqual(reply.Time, replyTime);
                Assert.AreEqual(reply.Text, replyText);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void SendMessage()
        {
            const string messageText = "Pew pew pew YEAH";
            var messageTime = new DateTime(2018, 12, 12, 0, 0, 0);

            using (var db = new WCIWTDbContext())
            {
                db.Users.Add(CreateSampleUser(1));
                db.Users.Add(CreateSampleUser(2));
                db.Messages.Add(new Message
                {
                    Id = ToGuid(1),
                    Seen = false,
                    Text = messageText,
                    Time = messageTime,
                    UserReceiverId = ToGuid(1),
                    UserSenderId = ToGuid(2),
                });
                db.SaveChanges();
            }

            using (var db = new WCIWTDbContext())
            {
                Assert.AreEqual(db.Users.Count(), 2);
                Assert.AreEqual(db.Messages.Count(), 1);
                var message = db.Messages.First();
                Assert.AreEqual(message.Id, ToGuid(1));
                Assert.IsFalse(message.Seen);
                Assert.AreEqual(message.Time, messageTime);
                Assert.AreEqual(message.Text, messageText);
                Assert.AreEqual(message.UserReceiverId, ToGuid(1));
                Assert.IsNotNull(message.UserReceiver);
                Assert.AreEqual(message.UserReceiver.Id, ToGuid(1));
                Assert.AreEqual(message.UserSenderId, ToGuid(2));
                Assert.IsNotNull(message.UserSender);
                Assert.AreEqual(message.UserSender.Id, ToGuid(2));
                db.SaveChanges();
            }
        }

        private static User CreateSampleUser(int id)
        {
            return new User
            {
                Id = ToGuid(id),
                Username = $"Sample username {id}",
                Birthdate = new DateTime(1900 + id, id % 12, id % 28),
                PasswordHash = "BEEF",
                Gender = Gender.ApacheHelicopter,
                IsAdmin = false,
            };
        }

        private static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}
