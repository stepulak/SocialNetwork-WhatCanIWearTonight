using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDatabase
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var db = new MyDbContext())
            {
                var user = new User
                {
                    Id = 1,
                    Username = "Miss Fortune",
                    PasswordHash = "FFAACBDFEED",
                    Birthdate = new DateTime(1999, 12, 12),
                    Gender = Gender.ApacheHelicopter,
                    IsAdmin = true
                };

                var user2 = new User
                {
                    Id = 2,
                    Username = "Sivir",
                    PasswordHash = "ABCDEF",
                    Birthdate = new DateTime(2012, 12, 21),
                    Gender = Gender.Female,
                    IsAdmin = false
                };

                db.Users.Add(user);
                db.Users.Add(user2);
                db.SaveChanges();

                var post = new Post
                {
                    Id = 1,
                    Time = new DateTime(2013, 11, 13, 13, 13, 13),
                    Text = "Is this bikini ok?",
                    Visibility = PostVisibility.FriendsOnly,
                    GenderRestriction = Gender.NoInformation,
                    HasAgeRestriction = false,
                    UserId = 1
                };

                db.Posts.Add(post);
                db.SaveChanges();

                var reply = new PostReply
                {
                    Id = 1,
                    PostId = 1,
                    Text = "No bikini best bikini",
                    Time = new DateTime(2013, 11, 14, 14, 14, 14),
                    UserId = 2
                };

                db.PostReplys.Add(reply);
                db.SaveChanges();

                var friendship = new Friendship
                {
                    FriendshipId = 1,
                    IsConfirmed = false,
                    ApplicantId = 2,
                    RecipientId = 1
                };

                db.Friendships.Add(friendship);
                db.SaveChanges();

                var img = new Image
                {
                    Id = 1,
                    BinaryImage = new byte[] { 0, 1, 2, 3 },
                    DislikesCount = 1,
                    LikesCount = 0,
                    PostId = 1,
                };

                db.Images.Add(img);
                db.SaveChanges();

                var vote = new Vote
                {
                    ImageId = 1,
                    UserId = 2,
                    Type = VoteType.Dislike,
                };

                db.Votes.Add(vote);
                db.SaveChanges();

                {
                    var query = db.Posts.First();
                    Console.WriteLine($"Post text: {query.Text} " +
                        $"Username: {query.User.Username}" +
                        $"First reply: {query.Replys.First().Text} " +
                        $"First reply user: {query.Replys.First().User.Username}");
                }
                {
                    var query = db.Friendships.First();
                    Console.WriteLine($"Friendship: First user: {query.Applicant.Username} Second user: {query.Recipient.Username}");
                }
                {
                    var query = db.Votes.First();
                    Console.WriteLine($"Vote: From: {query.User.Username } " +
                        $"Image id: {query.Image.Id} " +
                        $"Post text: {query.Image.Post.Text} ");
                }
            }
            Console.ReadKey();
        }
    }
}
