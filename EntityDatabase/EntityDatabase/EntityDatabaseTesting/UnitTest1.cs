using System;
using System.Linq;
using EntityDatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityDatabaseTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddSingleUser()
        {
            const string username = "Miss Fortune";
            const string passwd = "25afd84d";
            DateTime birthdate = new DateTime(1999, 12, 12);

            using (var db = new MyDbContext())
            {
                var user = new User
                {
                    Id = 1,
                    Username = username,
                    PasswordHash = passwd,
                    Birthdate = birthdate,
                    Gender = Gender.Female,
                    IsAdmin = false
                };
                db.Users.Add(user);
            }

            using (var db = new MyDbContext())
            {
                Assert.IsTrue(db.Users.Count() == 1);
                var user = db.Users.First();
                Assert.AreEqual(user.Id, 1);
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
                db.Users.Remove(user);
            }
        }

        [TestMethod]
        public void CreateFriendship()
        {
            using (var db = new MyDbContext())
            {
                db.Users.Add(CreateSampleUser(1));
                db.Users.Add(CreateSampleUser(2));
                db.Friendships.Add(new Friendship
                {
                    FriendshipId = 1,
                    ApplicantId = 1,
                    RecipientId = 2,
                    IsConfirmed = true
                });
            }

            using (var db = new MyDbContext())
            {
                Assert.IsTrue(db.Users.Count() == 2);
                Assert.IsTrue(db.Friendships.Count() == 1);
                var friendship = db.Friendships.First();
                Assert.AreEqual(friendship.ApplicantId, 1);
                Assert.AreEqual(friendship.RecipientId, 2);
                Assert.IsTrue(friendship.IsConfirmed);
                Assert.IsNotNull(friendship.Applicant);
                Assert.IsNotNull(friendship.Recipient);
                Assert.AreEqual(friendship.Applicant.Id, 1);
                Assert.AreEqual(friendship.Recipient.Id, 2);
                Assert.AreSame(friendship.Applicant, db.Users.ElementAt(0));
                Assert.AreSame(friendship.Recipient, db.Users.ElementAt(1));
                db.Users.RemoveRange(db.Users);
                db.Friendships.Remove(friendship);
            }
        }

        private User CreateSampleUser(int id)
        {
            return new User
            {
                Id = id,
                Username = $"Sample username {id}",
                Birthdate = new DateTime(1900 + id, id % 12, id % 28),
                PasswordHash = "BEEF",
                Gender = Gender.ApacheHelicopter,
                IsAdmin = false,
            };
        }
    }
}
