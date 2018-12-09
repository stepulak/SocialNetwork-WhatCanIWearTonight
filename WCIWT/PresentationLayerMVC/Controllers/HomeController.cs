using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.Facades;
using PresentationLayerMVC.Models.Aggregated;
using PresentationLayerMVC.Models.FriendRequests;
using PresentationLayerMVC.Models.Friends;
using PresentationLayerMVC.Models.Posts;
using X.PagedList;

namespace PresentationLayerMVC.Controllers
{
    public class HomeController : Controller
    {
        public const int PostsPageSize = 10;
        public const int FriendRequestsPageSize = 20;
        public const int FriendsPageSize = 20;
        public const int ImagesPerPost = 5;
        private const string FilterSessionKey = "filter";
        
        public UserFacade UserFacade { get; set; }
        public PostFacade PostFacade { get; set; }

        public PartialViewResult UserMenu()
        {
            return PartialView("_UserMenu");
        }

        public async Task<ActionResult> Index(int page = 1)
        {
            var userId = await GetGuidOfLoggedUser();
            var postsModel = await GetPostModel(userId, page);
            var friendRequestsModel = await GetFriendRequestsModel(userId);
            var friendsModel = await GetFriendsModel(userId, page);
            var homepageModel = new HomePageAggregatedViewModel
            {
                PostListViewModel = postsModel,
                FriendRequestListViewModel = friendRequestsModel,
                FriendListViewModel = friendsModel
            };
            return View("Index", homepageModel);
        }

        private async Task<FriendRequestListViewModel> GetFriendRequestsModel(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return new FriendRequestListViewModel
                {
                    FriendRequests = new StaticPagedList<FriendshipDto>(new List<FriendshipDto>(), 1, 0, 0)
                };
            }
            var friendshipRequests = await UserFacade.PendingFriendshipRequests(userId);
            var friendRequestsModel = InitializeFriendRequestListViewModel(friendshipRequests);
            return friendRequestsModel;
        }

        private async Task<FriendListViewModel> GetFriendsModel(Guid userId, int page)
        {

            if (userId == Guid.Empty)
            {
                return new FriendListViewModel
                {
                    Friends = new StaticPagedList<UserDto>(new List<UserDto>(), page, 0, 0)
                };
            }

            var filter = new FriendshipFilterDto()
            {
                IsConfirmed = true,
                PageSize = FriendsPageSize,
                RequestedPageNumber = page,
                UserA = userId
            };
            var friends = await UserFacade.GetFriendsOfUser(userId, filter);
            return InitializeFriendListViewModel(friends);
        }
        

        private async Task<PostListViewModel> GetPostModel(Guid userId, int page)
        {
            var filter = Session[FilterSessionKey] as PostFilterDto ?? new PostFilterDto{PageSize = PostsPageSize};
            filter.RequestedPageNumber = page;

            var posts = await PostFacade.GetPostFeedAsync(filter, userId);
            var imagesForPosts = new List<List<ImageDto>>();

            foreach (var post in posts.Items)
            {
                imagesForPosts.Add(await PostFacade.ListOfImagesForPost(post.Id));
            }

            return InitializePostListViewModel(posts, imagesForPosts);
        }

        private FriendRequestListViewModel InitializeFriendRequestListViewModel(
            QueryResultDto<FriendshipDto, FriendshipFilterDto> result)
        {
            return new FriendRequestListViewModel
            {
                FriendRequests = new StaticPagedList<FriendshipDto>(result.Items, result.RequestedPageNumber ?? 1, FriendRequestsPageSize,
                    (int) result.TotalItemsCount),
                Filter = result.Filter
            };
        }

        private PostListViewModel InitializePostListViewModel(QueryResultDto<PostDto, PostFilterDto> postsResult,
            List<List<ImageDto>> imagesResult)
        {
            return new PostListViewModel
            {
                Posts = new StaticPagedList<PostDto>(postsResult.Items, postsResult.RequestedPageNumber ?? 1, PostsPageSize,
                    (int)postsResult.TotalItemsCount),
                ImagesForPosts = imagesResult,
                PostFilter = postsResult.Filter,
            };
        }

        private FriendListViewModel InitializeFriendListViewModel(QueryResultDto<UserDto, FriendshipFilterDto> friends)
        {
            int page = friends.RequestedPageNumber ?? 1;
            return new FriendListViewModel
            {
                Friends = new StaticPagedList<UserDto>(
                    friends.Items,
                    page,
                    friends.PageSize,
                    (int)friends.TotalItemsCount),
                Filter = friends.Filter
            };
        }

        private async Task<Guid> GetGuidOfLoggedUser()
        {
            var user = await GetLoggedUser();
            return user?.Id ?? Guid.Empty;
        }

        private async Task<UserDto> GetLoggedUser()
        {
            return HttpContext.User?.Identity != null
                ? await UserFacade.GetUserByUsernameAsync(HttpContext.User.Identity.Name)
                : null;
        }
    }
}