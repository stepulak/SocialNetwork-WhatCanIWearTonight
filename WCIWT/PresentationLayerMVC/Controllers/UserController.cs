using BusinessLayer.Facades;
using PresentationLayerMVC.Models.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using PresentationLayerMVC.Models.Aggregated;
using PresentationLayerMVC.Models.Friends;
using PresentationLayerMVC.Models.Posts;
using X.PagedList;

namespace PresentationLayerMVC.Controllers
{
    [RoutePrefix("users")]
    public class UserController : Controller
    {
        public const int PostsPageSize = 5;
        public const int ImagesPerPost = 5;
        public const int FriendsPageSize = 20;

        public UserFacade UserFacade { get; set; }
        public PostFacade PostFacade { get; set; }
        public MessageFacade MessageFacade { get; set; }

        // GET: user/{username}
        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult> Index(string username, int page = 1)
        {
            var user = await UserFacade.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return UserNotFoundModel(username);
            }

            var friendshipWithLoggedUser = await GetFriendshipWithLoggedUser(user.Id);
            var posts = await GetPostModel(user.Id, page, ResolveIsFriendForModel(friendshipWithLoggedUser));
            var model = new UserProfileAggregatedViewModel()
            {
                User = user,
                PostListViewModel = posts,
                IsFriend = ResolveIsFriendForModel(friendshipWithLoggedUser),
                HasPendingFriendRequest = ResolveHasPendingFriendRequestForModel(friendshipWithLoggedUser),
                Page = page
            };
            return View($"UserProfileView", model);
        }

        // GET: user/{username}/friends
        [HttpGet]
        [Route("{username}/friends")]
        public async Task<ActionResult> DisplayFriends(string username, int page = 1)
        {
            var user = await UserFacade.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return UserNotFoundModel(username);
            }

            var friends = await GetUserFriendsModel(user, page);
            var model = new UserFriendsAggregatedViewModel()
            {
                User = user,
                UserFriendsList = friends,
                Page = page
            };
            return View("UserFriendsListView", model);
        }

        // POST: user/{username}/add-friend
        [HttpPost]
        [Route("{username}/add-friend")]
        public async Task<ActionResult> AddFriend(string username)
        {
            var loggedUser = await GetLoggedUser();
            var friendToAdd = await UserFacade.GetUserByUsernameAsync(username);
            if (loggedUser == null)
            {
                RedirectToLogin();
            }

            if (friendToAdd == null)
            {
                ModelState.AddModelError("User", "User does not exist!");
                return View();
            }

            if (!await UserFacade.CanSendFriendshipRequest(loggedUser, friendToAdd))
            {
                ModelState.AddModelError("User", "Cannot send friendship request to this user!");
                return View();
            }

            try
            {
                string url = Request.UrlReferrer.AbsoluteUri;
                await UserFacade.SendFriendshipRequest(loggedUser, friendToAdd);
                return Redirect(url);
            }
            catch (Exception)
            {
                ModelState.AddModelError("User", "Cannot send friendship request to this user!");
                return View();
            }
        }

        // POST: user/{username}/remove-friend
        [HttpPost]
        [Route("{username}/remove-friend")]
        public async Task<ActionResult> RemoveFriend(string username)
        {
            var loggedUser = await GetLoggedUser();
            var friendToRemove = await UserFacade.GetUserByUsernameAsync(username);
            if (loggedUser == null)
            {
                RedirectToLogin();
            }

            if (friendToRemove == null)
            {
                ModelState.AddModelError("User", "User does not exist!");
                return View();
            }

            var friendshipToRemove = await UserFacade.GetFriendshipBetweenUsers(loggedUser.Id, friendToRemove.Id);
            if (friendshipToRemove == null || !friendshipToRemove.IsConfirmed)
            {
                ModelState.AddModelError("User", "Cannot remove friendship with this user!");
                return View();
            }

            try
            {
                string url = Request.UrlReferrer.AbsoluteUri;
                await UserFacade.RemoveFriendship(friendshipToRemove);
                return Redirect(url);
            }
            catch (Exception)
            {
                ModelState.AddModelError("User", "Cannot remove friendship with this user");
                return View();
            }
        }

        // POST: user/{username}/confirm-friend
        [HttpPost]
        [Route("{username}/confirm-friend")]
        public async Task<ActionResult> ConfirmFriend(string username)
        {
            var loggedUser = await GetLoggedUser();
            var friendToConfirm = await UserFacade.GetUserByUsernameAsync(username);
            if (loggedUser == null)
            {
                RedirectToLogin();
            }

            if (friendToConfirm == null)
            {
                ModelState.AddModelError("User", "User does not exist!");
                return View();
            }

            var friendshipToConfirm = await UserFacade.GetFriendshipRequestBetweenUsers(loggedUser.Id, friendToConfirm.Id);
            if (friendshipToConfirm == null || friendshipToConfirm.IsConfirmed)
            {
                ModelState.AddModelError("User", "Cannot confirm friendship with this user!");
                return View();
            }

            try
            {
                string url = Request.UrlReferrer.AbsoluteUri;
                await UserFacade.ConfirmFriendshipRequest(friendshipToConfirm);
                return Redirect(url);
            }
            catch (Exception)
            {
                ModelState.AddModelError("User", "Cannot confirm friendship with this user");
                return View();
            }
        }

        // POST: user/{username}/decline-friend
        [HttpPost]
        [Route("{username}/decline-friend")]
        public async Task<ActionResult> DeclineFriend(string username)
        {
            var loggedUser = await GetLoggedUser();
            var userToDecline = await UserFacade.GetUserByUsernameAsync(username);
            if (loggedUser == null)
            {
                RedirectToLogin();
            }

            if (userToDecline == null)
            {
                ModelState.AddModelError("User", "User does not exist!");
                return View();
            }

            var friendshipToDecline = await UserFacade.GetFriendshipRequestBetweenUsers(loggedUser.Id, userToDecline.Id);
            if (friendshipToDecline == null || friendshipToDecline.IsConfirmed)
            {
                ModelState.AddModelError("User", "Cannot decline friendship with this user!");
                return View();
            }

            try
            {
                string url = Request.UrlReferrer.AbsoluteUri;
                await UserFacade.CancelFriendshipRequest(friendshipToDecline);
                return Redirect(url);
            }
            catch (Exception)
            {
                ModelState.AddModelError("User", "Cannot decline friendship with this user");
                return View();
            }
        }

        [HttpGet]
        [Route("remove-user/{userId}")]
        public async Task<ActionResult> RemoveUser(Guid userId)
        {
            var loggedUser = await GetLoggedUser();
            if (loggedUser != null && loggedUser.IsAdmin)
            {
                var posts = await PostFacade.GetPostsByUserId(new PostFilterDto { }, userId);
                var messages = await MessageFacade.AllMessages(userId);
                foreach(var post in posts.Items)
                {
                    await PostFacade.DeletePost(post.Id);
                }
                foreach(var msg in messages.Items)
                {
                    await MessageFacade.DeleteMessage(msg.Id);
                }
                await RemoveUserNoException(userId);
            }
            return RedirectToAction("Index", "Home");
        }

        private ActionResult UserNotFoundModel(string username)
        {
            return View("UserProfileView", new UserProfileAggregatedViewModel() {
                Found = false,
                User = new UserDto { Username = username }
            });
        }

        private async Task RemoveUserNoException(Guid userId)
        {
            try
            {
                await UserFacade.RemoveUser(userId);
            }
            catch (Exception)
            {
                // ignore
            }
        }

        private async Task<FriendshipDto> GetFriendshipWithLoggedUser(Guid userId)
        {
            var loggedUser = await GetLoggedUser();
            if (loggedUser == null)
            {
                return null;
            }
           var friendship = await UserFacade.GetFriendshipBetweenUsers(loggedUser.Id, userId);
            if (friendship == null)
            {
                return await UserFacade.GetFriendshipRequestBetweenUsers(loggedUser.Id, userId);
            }

            return friendship;
        }

        private async Task<PostListViewModel> GetPostModel(Guid userId, int page, bool isFriend)
        {
            var filter =  await CreateFilterForUserDetailPosts(isFriend);
            filter.RequestedPageNumber = page;

            QueryResultDto<PostDto, PostFilterDto> posts;
            var loggedUser = await GetLoggedUser();
            if (loggedUser != null)
            {
                posts = await PostFacade.GetPostsByUserIdForLoggedUser(filter, userId, loggedUser.Id);
            }
            else
            {
                posts = await PostFacade.GetPostsByUserId(filter, userId);
            }

            var imagesForPosts = new List<List<ImageDto>>();

            foreach (var post in posts.Items)
            {
                imagesForPosts.Add(await PostFacade.ListOfImagesForPost(post.Id));
            }

            return InitializePostListViewModel(posts, imagesForPosts);
        }

        private async Task<PostFilterDto> CreateFilterForUserDetailPosts(bool isFriend)
        {
            var result = new PostFilterDto
            {
                PageSize = PostsPageSize,
            };
            var loggedUser = await GetLoggedUser();
            if (loggedUser != null)
            {
                var age = (int)((DateTime.Now - loggedUser.Birthdate).TotalDays / 365.2425);
                result.UserAge = age;
                result.GenderRestriction = loggedUser.Gender;
                result.LoggedUserId = loggedUser.Id;
            }
            if (isFriend)
            {
                result.IncludePrivatePosts = true;
            }

            return result;
        }

        private PostListViewModel InitializePostListViewModel(QueryResultDto<PostDto, PostFilterDto> postsResult,
            List<List<ImageDto>> imagesResult)
        {
            return new PostListViewModel
            {
                Posts = new StaticPagedList<PostDto>(postsResult.Items, postsResult.RequestedPageNumber ?? 1, PostsPageSize,
                    (int)postsResult.TotalItemsCount),
                ImagesForPosts = imagesResult,
                HashtagIndices = postsResult.Items.Select(p => PostFacade.FindHashtagIndices(p.Text)).ToList(),
                PostFilter = postsResult.Filter,
            };
        }

        private async Task<UserFriendsListViewModel> GetUserFriendsModel(UserDto user, int page)
        {
            var filter = new FriendshipFilterDto
            {
                PageSize = FriendsPageSize,
                RequestedPageNumber = page
            };
            var friends = await UserFacade.GetFriendsOfUser(user.Id, filter);
            return new UserFriendsListViewModel
            {
                Friends = await InitializeFriendsListViewModel(friends),
                Filter = friends.Filter
            };
        }

        private async Task<IPagedList<UserFriendViewModel>> InitializeFriendsListViewModel(QueryResultDto<UserDto, FriendshipFilterDto> friends)
        {
            var userFriendViewModelList = new List<UserFriendViewModel>();
            var loggedUser = await GetLoggedUser();
            foreach (var friend in friends.Items)
            {
                var friendshipWithLoggedUser = await GetFriendshipWithLoggedUser(friend.Id);
                userFriendViewModelList.Add(new UserFriendViewModel
                {
                    User = friend,
                    IsFriendWithLoggedUser = loggedUser != null
                                             && ResolveIsFriendForModel(friendshipWithLoggedUser),
                    HasPendingFriendshipRequest = loggedUser != null
                                                  && ResolveHasPendingFriendRequestForModel(friendshipWithLoggedUser)
                });
            }

            var page = friends.RequestedPageNumber ?? 1;
            return new StaticPagedList<UserFriendViewModel>(userFriendViewModelList, page, friends.PageSize,
                (int)friends.TotalItemsCount);

        }

        private async Task<UserDto> GetLoggedUser()
        {
            return HttpContext.User?.Identity != null
                ? await UserFacade.GetUserByUsernameAsync(HttpContext.User.Identity.Name)
                : null;
        }

        private void RedirectToLogin() => RedirectToAction("Login", "Account");

        private bool ResolveHasPendingFriendRequestForModel(FriendshipDto friendship) => friendship != null && !friendship.IsConfirmed;

        private bool ResolveIsFriendForModel(FriendshipDto friendship) => friendship != null && friendship.IsConfirmed;
    }
}